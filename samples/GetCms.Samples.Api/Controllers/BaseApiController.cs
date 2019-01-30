using GetCms.Models.General;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetCms.Samples.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        internal ILogger _logger { get; set; }


        internal string GetCurrentUserName()
        {
            //return User.Identity.Name;
            return "dev@test.com";
        }


        internal void ThrowException(Result result)
        {
            string message = string.Empty;
            if (result.Errors.Count > 0)
            {
                message = result.Errors[0].Message;
            }
            else // validation error
            {
                message = result.ValiationErrors[0].Message;
            }

            throw new Exception(message);
        }
    }
}
