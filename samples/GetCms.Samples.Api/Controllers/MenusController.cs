using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GetCms.Models;
using GetCms.Models.General;
using GetCms.Models.Services;
using GetCms.Samples.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GetCms.Samples.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : BaseApiController
    {
        private readonly IMenusService _menuService;
        public MenuController(ILogger<EmailTemplatesController> logger, 
            IMenusService menuService)
        {
            this._menuService = menuService;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<List<Menu>> Get(int? siteId = null, int? id = null, bool? isActive = null, string name = null, int from = 0, int to = 1)
        {
            return await _menuService.GetByAsync(siteId, id, isActive, name, from, to);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Menu menu)
        {
            Result result = null;
            try
            {
                result = await _menuService.SaveAsync(menu, GetCurrentUserName());

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
        public async Task<ActionResult> Put(Menu menu)
        {
            Result result = null;
            try
            {
                result = await _menuService.SaveAsync(menu, GetCurrentUserName());

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
                result = await _menuService.RemoveAsync(id);
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