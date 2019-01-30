using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetCms.Models;
using GetCms.Models.Cms.Extensions;
using GetCms.Models.DataAccess;
using GetCms.Models.Enums;
using GetCms.Models.General;
using GetCms.Models.Services;
using GetCms.Models.Validation;
using Microsoft.Extensions.Logging;

namespace GetCms.Services
{
    /// <summary>
    /// Methods 
    /// </summary>
    public class SitesService : BaseService, ISiteService
    {
        private static ConcurrentDictionary<int, Site> _sites = new ConcurrentDictionary<int, Site>();
        private static ConcurrentDictionary<string, int> _sitesNames = new ConcurrentDictionary<string, int>();

        private readonly ISitesDataAccess _sitesDataAccess;
        private readonly IValidator<Site> _validator;
        public SitesService(ILoggerFactory loggerFactory, 
            ISitesDataAccess sitesDataAccess, 
            IValidator<Site> validator) : base(loggerFactory)
        {
            _sitesDataAccess = sitesDataAccess;
            _validator = validator;
        }

        public async Task<List<Site>> GetByAsync(int? id = null, bool? isActive = null, string name = null, int? from = 0, int? to = 10)
        {
            return await _sitesDataAccess.GetByAsync(id, null, isActive, name, null, null, from, to);
        }

        public async Task<Site> GetByIdAsync(int siteId)
        {
            if (_sites.ContainsKey(siteId))
                return _sites[siteId];

            var list = await _sitesDataAccess.GetByAsync(siteId, null, null, null, null, null, 0, 1);

            if (list.Count > 0)
                return list[0];

            return null;
        }

        public async Task<Site> GetByNameAsync(string name)
        {
            if (_sitesNames.ContainsKey(name))
                return _sites[_sitesNames[name]];

            var list = await _sitesDataAccess.GetByAsync(null, null, true, name, null, null, 0, 1);

            if (list.Count > 0)
                return list[0];

            return null;
        }

        public async Task<Site> GetDefaultSiteAsync()
        {
            // two possible implametations
            // 1. detect site by host and language 2. take from confing

            var site = await GetByNameAsync("Demo");

            // cache

            return site; 
        }

        public async Task<Result> SaveAsync(Site site, string username)
        {
            site.Audit(username);
            // validate
            var result = _validator.Validate(site);

            

            if (result.IsValid)
            {
                try
                {
                    site.Id = result.NewId = await _sitesDataAccess.SaveAsync(site, site.IsNew ? DataAccessActions.Insert : DataAccessActions.Update);
                    
                }
                catch (Exception ex)
                {
                    _logger.LogError($"SaveAsync error: {ex.Message}, stack: {ex.StackTrace}");
                    result.Errors.Add(new ErrorItem(ex.Message));
                }
            }

            return result;
        }

        public async Task LoadSites()
        {
            // ToDo: load all sites
            var sites = await _sitesDataAccess.GetByAsync(null, null, true, null, null, null, 0, 10);

            foreach (var site in sites)
            {
                if (!_sites.ContainsKey(site.Id))
                {
                    _sites.AddOrUpdate(site.Id, site, (key, oldValue) => site);
                    _sitesNames.AddOrUpdate(site.Name, site.Id, (key, oldValue) => site.Id);
                }
            }
        }

        public async Task<Site> DetectSiteAsync(string host, string lang)
        {
            // by host and language detection logic
            // since we have only one site
            return await GetDefaultSiteAsync();

            
        }

        public List<int> GetActiveSites() => _sites.Keys.ToList();
    }
}
