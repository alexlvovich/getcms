using GetCms.Models;
using GetCms.Models.Cms.Extensions;
using GetCms.Models.DataAccess;
using GetCms.Models.Enums;
using GetCms.Models.General;
using GetCms.Models.Services;
using GetCms.Models.Validation;
using GetCms.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GetCms.Services.Cms
{
    public class ContentService : BaseService, IContentService
    {
        private readonly IContentsDataAccess _contentDataAccess;
        private readonly IValidator<Content> _validator;
        public ContentService(ILoggerFactory loggerFactory, 
            IContentsDataAccess contentDataAccess,
            IValidator<Content> validator)
            : base(loggerFactory)
        {
            this._contentDataAccess = contentDataAccess;
            this._validator = validator;
        }

        public async Task<Result> SaveAsync(Content content, string username)
        {
            content.Audit(username);
            bool isNew = content.IsNew;
            var result = _validator.Validate(content);

            if (result.IsValid)
            {
                try
                {
                    result.NewId = await _contentDataAccess.SaveAsync(content, content.IsNew ? DataAccessActions.Insert : DataAccessActions.Update);

                    if (!result.IsValid) throw new Exception("");

                    if (content.Type == ContentTypes.EmailTemplate)
                    {
                        var t = content as MessagingTemplate;
                        if (isNew)
                        {
                            t.ContentId = result.NewId;
                        }
                            

                        await _contentDataAccess.SaveEmailTemplateAsync(t, isNew ? DataAccessActions.Insert : DataAccessActions.Update);
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

        private string GetTemplateParameters(string str)
        {
            var regex = new Regex(@"[\{\#\#(+.)\#\#\}]");
            StringBuilder sb = new StringBuilder();

            var matches = regex.Matches(str);

            foreach (Match match in matches)
            {
                sb.AppendFormat("{0}, ", match.Groups[0].Value);
            }

            return sb.ToString();
        }

        public async Task<Content> GetByIdAsync(int contentId)
        {
            var result = await GetByAsync(null, contentId, null, 0, 1);

            if (result.Total > 0)
                return result.List[0];

            return null;
        }

        public async Task<PagedResults<Content>> GetByAsync(int? siteId= null, int? id = null, int? pageId = null, int from = 0, int to = 10)
        {
            return await _contentDataAccess.GetByAsync(siteId, id, pageId, from, to);
        }

        public async Task<Result> MapToPage(int contentId, int pageId, bool unmap = false)
        {
            var result = new Result();

            try
            {
                if (!unmap)
                    await _contentDataAccess.SaveMapping(contentId, pageId, DataAccessActions.Insert);
                else
                {
                    await _contentDataAccess.SaveMapping(contentId, pageId, DataAccessActions.Delete);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"SaveAsync error: {ex.Message}, stack: {ex.StackTrace}");
                result.Errors.Add(new ErrorItem(ex.Message));
            }

            

            return result;
        }



        public async Task<PagedResults<MessagingTemplate>> GetEmailTemplatesByAsync(int? siteId = null, int? id = null, byte? templateType = null, byte? target = null, bool? isActive = null, int from = 0, int to = 10)
        {
            var result = await _contentDataAccess.GetEmailTemplatesByAsync(siteId, id, templateType, target, isActive, from, to);

            return result;
        }

        public async Task<MessagingTemplate> GetEmailTemplateByIdAsync(int templateId)
        {
            var res = await GetEmailTemplatesByAsync(null, templateId, null, null, null, 0, 1);

            if (res.Total > 0)
                return res.List[0];

            return null;
        }

        public async Task<Result> RemoveAsync(int id)
        {
            var result = new Result();

            try
            {
                var content = await GetByIdAsync(id);

                await _contentDataAccess.SaveAsync(content, DataAccessActions.Delete);

            }
            catch (Exception ex)
            {
                _logger.LogError($"SaveAsync error: {ex.Message}, stack: {ex.StackTrace}");
                result.Errors.Add(new ErrorItem(ex.Message));
            }

            return result;
        }

    }
}
