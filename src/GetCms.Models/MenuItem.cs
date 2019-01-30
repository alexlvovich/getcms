using System;
using Newtonsoft.Json;

namespace GetCms.Models
{
    public class MenuItem : Auditable
    {
        public int MenuItemId { get; set; }
        public int MenuId { get; set; }

        public string Text { get; set; }

        public string Alt { get; set; }

        public string Link { get; set; }

        public byte Order { get; set; }

        public bool IsActive { get; set; }

        public string ImagePath { get; set; }

        public string ImagePathAlt { get; set; }

    }
}
