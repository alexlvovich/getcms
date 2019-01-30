using GetCms.Models.General;
using System;

namespace GetCms.Models
{
    /// <summary>
    /// Represents site object
    /// </summary>
    public class Site : Auditable
    {
        public string Name { get; set; }
        public string Host { get; set; }

        public Language Language { get; set; }


        public bool IsActive { get; set; }
        public int TimeZoneOffset { get; set; }

        public string PageTitleSeparator { get; set; }

        public string ContentType { get; set; }

        public int? ParentSiteId { get; set; }
    }
}
