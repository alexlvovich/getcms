using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetCms.Models;
using GetCms.Models.Cms.Enums;
using GetCms.Models.Cms.Extensions;
using GetCms.Models.DataAccess;
using GetCms.Models.Enums;
using GetCms.Models.General;
using GetCms.Models.Services;
using GetCms.Models.Validation;
using GetCms.Services;
using Microsoft.Extensions.Logging;

namespace GetCms.Services.Cms
{
    public class PagesService : BaseService,  IPageService
    {
        private readonly IPagesDataAccess _pageDataAccess;
        private readonly IMetasService _metaService;
        private readonly IContentService _contentService;
        private readonly IValidator<Page> _validator;
        private readonly ISiteService _siteService;

        private static ConcurrentDictionary<Tuple<int, string>, Page> _pages = new ConcurrentDictionary<Tuple<int, string>, Page>();

        public PagesService(ILoggerFactory loggerFactory, 
            IPagesDataAccess pageDataAccess,
            IMetasService metaService,
            IContentService contentService,
            IValidator<Page> validator, 
            ISiteService siteService)
            : base(loggerFactory)
        {
            this._pageDataAccess = pageDataAccess;
            this._validator = validator;
            this._metaService = metaService;
            this._contentService = contentService;
            this._siteService = siteService;
        }

        public async Task<PagedResults<Page>> GetByAsync(int? siteId, int? id = null, string name = null, string slug = null, bool? published = null, bool? active = null, int? parentId = null, byte? type = null, int from = 0, int to = 10)
        {
            var result = await _pageDataAccess.GetByAsync(siteId, id, name, slug, published, active, parentId, type, from, to);

            foreach (var p in result.List)
            {
                var contents = await _contentService.GetByAsync(null, null, p.Id, 0, 10);
                p.Contents = contents.List.OrderBy(c=>c.Order).ToList();

                p.MetaData = await _metaService.GetAsync(p.Id, p.SiteId);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<Result> SaveAsync(Page page, string userName)
        {
            page.Audit(userName);
            bool isNew = page.IsNew;
            var result = _validator.Validate(page);
            
            if (result.IsValid)
            {
                try
                {
                    page.Id = result.NewId = await _pageDataAccess.SaveAsync(page, page.IsNew ? DataAccessActions.Insert : DataAccessActions.Update);
                    if (page.Contents != null && page.Contents.Count > 0)
                    {
                        foreach (var c in page.Contents)
                        {
                            // save content
                            c.SiteId = page.SiteId;
                            var res = await _contentService.SaveAsync(c, userName);

                            if (!res.Succeeded)
                                throw new Exception(res.Errors[0].Message);

                            c.Id = res.NewId;

                            if (isNew)
                            {
                                var resMapping = await _contentService.MapToPage(c.Id, page.Id);
                                if (!resMapping.Succeeded)
                                    throw new Exception(resMapping.Errors[0].Message);
                            }
                        }
                    }
                    

                    // save meta data
                    if (null != page.MetaData && page.MetaData.Count > 0)
                    {
                        foreach (var metaKey in page.MetaData.Keys)
                        {
                            var m = page.MetaData[metaKey];
                            if (m.IsNew)
                            {
                                m.Key = metaKey;
                                m.ItemId = page.Id;
                                m.Type = MetaDataTypes.Page;
                                m.CreatedOn = DateTime.Now;
                                m.CreatedBy = userName;
                                m.SiteId = page.SiteId;
                            }
                            await _metaService.SaveAsync(m, userName);
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError($"SaveAsync error: {ex.Message}, stack: {ex.StackTrace}");
                    result.Errors.Add(new ErrorItem(ex.Message));
                }
            }

            return result;
        }

        public async Task<Page> GetByIdAync(int pageId)
        {
            var result = await GetByAsync(null, pageId, null, null, null, null, 0, 1);

            if (result.Total > 0)
                return result.List[0];

            return null;
        }

        public async Task<Page> GetByName(string name, int siteId)
        {
            var result = await GetByAsync(siteId, null, name, null, null, null, 0, 1);

            if (result.Total > 0)
                return result.List[0];

            return null;
        }
        public async Task<Page> GetBySlug(string slug, int siteId)
        {
            var t = new Tuple<int, string>(siteId, slug);
            if (_pages.ContainsKey(t))
                return _pages[t];

            var result = await GetByAsync(siteId, null, null, slug, null, null, 0, 1);

            if (result.Total > 0)
                return result.List[0];

            return null;
        }

        public async Task<Result> RemoveAsync(int pageId)
        {
            var result = new Result();

            try
            {
                var page = await GetByIdAync(pageId);

                await _pageDataAccess.SaveAsync(page, DataAccessActions.Delete);

            }
            catch (Exception ex)
            {
                _logger.LogError($"SaveAsync error: {ex.Message}, stack: {ex.StackTrace}");
                result.Errors.Add(new ErrorItem(ex.Message));
            }

            return result;
        }

        public async Task LoadPages()
        {
            // load all pages by active sites
            var sites = _siteService.GetActiveSites();

            foreach (var siteId in sites)
            {
                int count = 0;
                long total = 0;
                
                while (count < total || count == 0)
                {
                    var pagesRes = await GetByAsync(siteId, null, null, null, null, true, null, null, count, count+10);

                    if (total == 0)
                        total = pagesRes.Total;

                    foreach (var page in pagesRes.List)
                    {
                        var t = new Tuple<int, string>(siteId, page.Slug);

                        _pages.AddOrUpdate(t, page, (key, oldValue) => page);

                        count++;
                    }
                }

            }

        }
    }
}
