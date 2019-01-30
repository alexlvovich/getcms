using GetCms.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GetCms.Models.DataAccess
{
    public interface IMenusDataAccess
    {
        Task<int> SaveAsync(Menu menu, DataAccessActions DataAccessActions);

        Task<List<Menu>> GetByAsync(int? siteId = null, int? id = null, bool? isActive = null, string name = null, int from = 0, int to = 10);

        Task<int> SaveItemAsync(MenuItem menuItem, DataAccessActions DataAccessActions);

        Task<List<MenuItem>> GetItemsByAsync(int? menuId, int? id);
    }
}
