using GetCms.Models;
using GetCms.Models.General;
using GetCms.Models.Services;
using GetCms.Models.Validation;
using GetCms.Samples.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace GetCms.Samples.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentsController : BaseApiController
    {
        private readonly IContentService contentService;
        public ContentsController(ILogger<ContentsController> logger, 
            IContentService contentService)
        {
            this.contentService = contentService;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<PagedResults<Content>> Get(int? contentId = null, int? siteId = null, int? pageId = null, int from = 0, int to = 1)
        {
            return await contentService.GetByAsync(siteId, contentId, pageId, from, to);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Content content)
        {
            Result result = null;
            try
            {
                result = await contentService.SaveAsync(content, GetCurrentUserName());

                if (!result.Succeeded)
                    ThrowException(result);

                return Created("", result.NewId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"POST Error: {ex.Message}, Stack: {ex.StackTrace}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put(Content content)
        {
            Result result = null;
            try
            {
                result = await contentService.SaveAsync(content, GetCurrentUserName());

                if (!result.Succeeded)
                    ThrowException(result);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"PUT Error: {ex.Message}, Stack: {ex.StackTrace}");
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            Result result = null;
            try
            {
                result = await contentService.RemoveAsync(id);
                if (!result.Succeeded)
                    ThrowException(result);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"DELETE Error: {ex.Message}, Stack: {ex.StackTrace}");
                return BadRequest(ex.Message);
            }
        }
    }
}