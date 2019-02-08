using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GetCms.Models;
using GetCms.Models.General;
using GetCms.Models.Services;
using GetCms.Models.Validation;
using GetCms.Samples.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GetCms.Samples.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagesController : BaseApiController
    {
        private readonly IPageService _pageService;
        private readonly IValidator<Page> validator;
        public PagesController(ILogger<ContentsController> logger, 
            IPageService pageService, 
            IValidator<Page> validator)
        {
            this._pageService = pageService;
            this.validator = validator;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<PagedResults<Page>> Get(int? siteId = null, string name = null, 
            string slug = null, int? id = null, bool? published = true, 
            bool? active = true, int? parentId = null, byte? type = null, 
            int from = 0, int to = 20)
        {
            return await _pageService.GetByAsync(siteId, id, name, slug, published, active, parentId, type, from, to);
        }


        [HttpPost]
        public async Task<ActionResult> Post(Page page)
        {
            Result result = null;
            try
            {
                result = await _pageService.SaveAsync(page, GetCurrentUserName());

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
        public async Task<ActionResult> Put(Page page)
        {
            Result result = null;
            try
            {
                result = await _pageService.SaveAsync(page, GetCurrentUserName());

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
                result = await _pageService.RemoveAsync(id);
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
