using System;

namespace GetCms.Models
{
    public abstract class Auditable : IAuditable
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string PublishedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

        public bool IsNew
        {
            get { return Id == 0; }
        }
    }
}
