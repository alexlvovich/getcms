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
    public class MenuItemsController: BaseApiController
    {
        private readonly IMenusService _menuService;
        public MenuItemsController(ILogger<MenuItemsController> logger, 
            IMenusService menuService)
        {
            this._menuService = menuService;
        }

        [HttpGet]
        public async Task<List<MenuItem>> Get(int? menuId = null, int? id = null)
        {
            return await _menuService.GetItemsAsync(menuId, id);
        }


        [HttpPost]
        public async Task<ActionResult> Post(MenuItem menuItem)
        {
            Result result = null;
            try
            {
                result = await _menuService.SaveItemAsync(menuItem, GetCurrentUserName());

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
        public async Task<ActionResult> Put(MenuItem menuItem)
        {
            Result result = null;
            try
            {
                result = await _menuService.SaveItemAsync(menuItem, GetCurrentUserName());

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