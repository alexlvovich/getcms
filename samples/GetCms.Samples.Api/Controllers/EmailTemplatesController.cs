using GetCms.Models;
using GetCms.Models.Enums;
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
    public class EmailTemplatesController : BaseApiController
    {
        private readonly IContentService _contentService;
   
        public EmailTemplatesController(ILogger<EmailTemplatesController> logger,
            IContentService contentService)
        {
            this._contentService = contentService;
            this._logger = logger;
        }


        [HttpGet]
        public async Task<PagedResults<MessagingTemplate>> Get(int? siteId = null, int? contentId = null, byte? templateTypeId = null, byte? targetId = null, bool? isActive = null, int from = 0, int to = 1)
        {
            return await _contentService.GetEmailTemplatesByAsync(siteId, contentId, templateTypeId, targetId, isActive, from, to);
        }

        [HttpPost]
        public async Task<ActionResult> Post(MessagingTemplate content)
        {
            Result result = null;
            try
            {
                content.Type = ContentTypes.EmailTemplate;
                result = await _contentService.SaveAsync(content, GetCurrentUserName());

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
        public async Task<ActionResult> Put(MessagingTemplate content)
        {
            Result result = null;
            try
            {
                result = await _contentService.SaveAsync(content, GetCurrentUserName());

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
                result = await _contentService.RemoveAsync(id);
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