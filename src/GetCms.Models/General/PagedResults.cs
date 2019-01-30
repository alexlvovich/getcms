using System;
using System.Collections.Generic;
using System.Text;

namespace GetCms.Models.General
{
    public class PagedResults<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public List<T> List { get; set; }

        /// <summary>
        /// /
        /// </summary>
        public long Total { get; set; }
    }
}
