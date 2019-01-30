using GetCms.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetCms.Samples.WebApp.Infrastructure
{
    public class AsyncSiteDetectionActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var site = new Site()
            {
                Name = "default",
                Id = 1,
                Language = Languages.English
            };

            context.HttpContext.Items.Add("site", site);
            // execute any code before the action executes
            var result = await next();
            // execute any code after the action executes
        }
    }
}
