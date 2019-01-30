using GetCms.Models;
using GetCms.Models.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetCms.Samples.WebApp.Infrastructure
{
    public class GetCmsViewLocator : IViewLocationExpander
    {
        public readonly ISiteService _siteService;

        public GetCmsViewLocator(ISiteService siteService)
        {
            _siteService = siteService;
        }
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            Site site = _siteService.DetectSiteAsync(context.ActionContext.HttpContext.Request.Host.Host, "en").Result;

            string folderName = $"{site.Name.ToLower()}-{site.Language.Code}";

            var paths = new string[] {
                $"~/Skins/{folderName}/views/{context.ControllerName}/{context.ViewName}.cshtml",
                $"~/Skins/{folderName}/views/{context.ControllerName}/{context.ViewName}.cshtml",
                $"~/Skins/{folderName}/views/shared/{context.ViewName}.cshtml",
                $"~/Skins/{folderName}/views/shared/{context.ViewName}.cshtml",
            };


            return paths.Concat(viewLocations);
          
            ////Replace folder view with CustomViews
            //return viewLocations.Select(f => f.Replace("/Views/", $"/Skins/{folderName}/"));
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {

        }
    }
}
