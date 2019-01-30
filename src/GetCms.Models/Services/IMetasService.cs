using GetCms.Models.General;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GetCms.Models.Services
{
    public interface IMetasService
    {
        Task<Result> SaveAsync(MetaData metaData, string username);

        Task<Dictionary<string, MetaData>> GetAsync(int itemId, int siteId);
    }
}
