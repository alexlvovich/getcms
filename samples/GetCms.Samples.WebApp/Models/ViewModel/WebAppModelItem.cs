using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetCms.Samples.WebApp.Models.ViewModel
{
    public class WebAppModelItem<T> : WebAppModel
    {
        public T Item { get; set; }
    }
}
