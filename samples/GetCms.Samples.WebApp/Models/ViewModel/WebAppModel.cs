using GetCms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetCms.Samples.WebApp.Models.ViewModel
{
    public class WebAppModel
    {
        public Site Site { get; set; }
        public Dictionary<string, Menu> Menus { get; set; }
    }
}
