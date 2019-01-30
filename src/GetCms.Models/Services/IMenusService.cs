using GetCms.Models.General;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GetCms.Models.Services
{
    public interface IMenusService
    {
        Task<List<Menu>> GetByAsync(int? siteId = null, int? id = null, bool? isActive = null, string name = null, int from = 0, int to = 10);
        
        Task<Menu> GetByIdAsync(int menuId);
        Task<Result> SaveAsync(Menu menu, string userName);

        Task<Result> RemoveAsync(int menuId);

        Task<Result> SaveItemAsync(MenuItem menuItem, string userName);

        Task<Result> RemoveItemAsync(int menuItemId);

        Task<List<MenuItem>> GetItemsAsync(int? menuId = null, int? id = null);

        Task<MenuItem> GetItemByIdAsync(int menuItemId);

        Task LoadMenus();

        Task<List<Menu>> GetBySiteIdAsync(int siteId);
    }
}
