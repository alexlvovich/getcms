using System;
using GetCms.Models.Cms.Enums;

namespace GetCms.Models
{
    public class MetaData : Auditable, IBelong
    {
        public int SiteId { get; set; }

        public MetaDataTypes Type { get; set; }
        
        public int ItemId { get; set; }

        public string Value { get; set; }

        public string Key { get; set; }

    }
}
