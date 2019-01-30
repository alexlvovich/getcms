using System;
using GetCms.Models.Enums;

namespace GetCms.Models
{
    /// <summary>
    /// Represents content object
    /// </summary>
    public class Content : Auditable, IBelong
    {
        public string Name { get; set; }
        public string Body { get; set; }

        public string Title { get; set; }

        public bool IsActive { get; set; }

        public ContentTypes Type { get; set; }
        
        public int SiteId { get; set; }
        
        public int? Order { get; set; }
    }
}
