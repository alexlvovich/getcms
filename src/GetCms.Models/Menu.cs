using System.Collections.Generic;

namespace GetCms.Models
{
    /// <summary>
    /// Represents a menu object 
    /// </summary>
    public class Menu : Auditable, IBelong
    {
        public int MenuId { get; set; }
        public List<MenuItem> Items { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
        public List<int> Pages { get; set; }
        public int SiteId { get; set; }
    }
}
