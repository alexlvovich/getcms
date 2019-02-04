using System;
using System.Collections.Generic;
using GetCms.Models.Cms.Enums;
using Newtonsoft.Json;

namespace GetCms.Models
{
    public class Page : Auditable, IBelong
    {
        public int? ParentId { get; set; }
        public PageTypes PageType { get; set; }
        public int? MasterPageId { get; set; }
        public Dictionary<string,MetaData> MetaData { get; set; }
        public string Css { get; set; }
        public bool IsActive { get; set; }


        public string Name { get; set; }
        public string Slug { get; set; }

        public int SiteId { get; set; }

        public string Title { get; set; }


        public List<Content> Contents { get; set; }

        [JsonIgnore]
        public DateTime ActualDate {
            get
            {
                if (ModifiedOn.HasValue)
                    return ModifiedOn.Value;
                return CreatedOn;

            }
        }

        [JsonIgnore]
        public Page MasterPage { get; set; }
    }
}
