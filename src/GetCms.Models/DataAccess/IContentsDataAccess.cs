using GetCms.Models.Enums;
using GetCms.Models.General;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GetCms.Models.DataAccess
{
    public interface IContentsDataAccess
    {
        Task<PagedResults<Content>> GetByAsync(int? siteId = null, int? id = null, int? pageId = null, int from = 0, int to = 10);

        Task<int> SaveAsync(Content content, DataAccessActions DataAccessActions);

        Task SaveMapping(int contentId, int pageId, DataAccessActions action);

        Task<int> SaveEmailTemplateAsync(MessagingTemplate t, DataAccessActions DataAccessActions);

        Task<PagedResults<MessagingTemplate>> GetEmailTemplatesByAsync(int? siteId = null, int? id = null, byte? templateType = null, byte? target = null, bool? isActive = null, int from = 0, int to = 10);
    }
}
