using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetCms.Samples.WebApp.Infrastructure.Constrains
{
    public class IsLanguage : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return (values[routeKey] != null
                   && values[routeKey].ToString().Length == 2);
        }
    }
}
