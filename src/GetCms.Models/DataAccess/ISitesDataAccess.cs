using System.Collections.Generic;
using System.Threading.Tasks;
using GetCms.Models.Enums;

namespace GetCms.Models.DataAccess
{
    public interface ISitesDataAccess
    {
        Task<int> SaveAsync(Site site, DataAccessActions insert);
        Task<List<Site>> GetByAsync(int? siteId, int? parentSiteId, bool? isActive, string name, string slug, string lang, int? from, int? to);
    }
}
