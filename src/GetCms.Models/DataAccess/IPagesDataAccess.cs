using GetCms.Models.Enums;
using GetCms.Models.General;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GetCms.Models.DataAccess
{
    public interface IPagesDataAccess
    {
        Task<int> SaveAsync(Page page, DataAccessActions action);

        Task<PagedResults<Page>> GetByAsync(int? siteId = null, int? id = null, string name = null, string slug = null, int from = 0, int to = 10);
    }
}
