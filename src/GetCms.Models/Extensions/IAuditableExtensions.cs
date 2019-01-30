using System;

namespace GetCms.Models.Cms.Extensions
{
    public static class IAuditableExtensions
    {
        public static void Audit(this IAuditable item, string userName)
        {
            if (!item.IsNew)
            {
                item.ModifiedBy = userName;
                item.ModifiedOn = DateTime.Now;
            }
            else
            {
                item.CreatedBy = userName;
                item.CreatedOn = DateTime.Now;
            }

        }
    }
}
