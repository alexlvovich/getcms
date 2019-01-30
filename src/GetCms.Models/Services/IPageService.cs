using GetCms.Models.General;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GetCms.Models.Services
{
    public interface IPageService
    {
        Task<PagedResults<Page>> GetByAsync(int? siteId, int? id = null, string name = null, string slug = null, bool? published = null, bool? active = null, int from = 0, int to = 10);

        Task<Result> SaveAsync(Page page, string username);

        Task<Page> GetByIdAync(int pageId);
        Task<Page> GetBySlug(string slug, int siteId);

        Task<Page> GetByName(string name, int siteId);

        Task<Result> RemoveAsync(int pageId);
        Task LoadPages();
    }
}
