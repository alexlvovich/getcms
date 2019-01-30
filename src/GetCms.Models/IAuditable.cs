using System;

namespace GetCms.Models
{
    public interface IAuditable
    {
        int Id { get; set; }
        DateTime CreatedOn { get; set; }
        string CreatedBy { get; set; }

        DateTime? PublishedOn { get; set; }
        string PublishedBy { get; set; }

        DateTime? ModifiedOn { get; set; }
        string ModifiedBy { get; set; }

        bool IsNew { get; }
    }
}
