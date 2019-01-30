using GetCms.Models.Cms.Enums;
using GetCms.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GetCms.Models.DataAccess
{
    public interface IMetasDataAccess
    {
        Task<List<MetaData>> GetAsync(int itemId, int siteId, MetaDataTypes? types);

        Task<int> SaveAsync(MetaData data, DataAccessActions action);
    }
}
