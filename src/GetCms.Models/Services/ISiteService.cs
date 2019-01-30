using GetCms.Models.General;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GetCms.Models.Services
{
    public interface ISiteService
    {
        Task<Result> SaveAsync(Site site, string username);
        Task<List<Site>> GetByAsync(int? id = null, bool? isActive = null, string name = null, int? from = 0, int? to = 10);
        Task<Site> GetByIdAsync(int siteId);
        Task<Site> GetByNameAsync(string name);
        Task<Site> GetDefaultSiteAsync();
        Task LoadSites();
        Task<Site> DetectSiteAsync(string host, string lang);

        List<int> GetActiveSites();
    }
}
