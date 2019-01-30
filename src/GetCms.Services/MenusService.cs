using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using GetCms.Models;
using GetCms.Models.Cms.Extensions;
using GetCms.Models.DataAccess;
using GetCms.Models.Enums;
using GetCms.Models.General;
using GetCms.Models.Services;
using GetCms.Models.Validation;
using GetCms.Services;
using Microsoft.Extensions.Logging;

namespace GetCms.Services.Cms
{
    public class MenusService : BaseService, IMenusService
    {
        private static ConcurrentDictionary<int, List<Menu>> _menus = new ConcurrentDictionary<int, List<Menu>>();
        private readonly IMenusDataAccess _menuDataAccess;
        private readonly IValidator<Menu> _validator;
        private readonly IValidator<MenuItem> _itemValidator;

        #region .CTOR
        public MenusService(ILoggerFactory loggerFactory,
            IValidator<Menu> validator,
            IValidator<MenuItem> itemValidator,
            IMenusDataAccess menuDataAccess)
            : base(loggerFactory)
        {
            this._menuDataAccess = menuDataAccess;
            this._validator = validator;
            this._itemValidator = itemValidator;
        } 
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<Result> SaveAsync(Menu menu, string username)
        {
            menu.Audit(username);

            var result = _validator.Validate(menu);

            if (result.IsValid)
            {
                try
                {
                    menu.Id = result.NewId = await _menuDataAccess.SaveAsync(menu, menu.IsNew ? DataAccessActions.Insert : DataAccessActions.Update);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"SaveAsync error: {ex.Message}, stack: {ex.StackTrace}");
                    result.Errors.Add(new ErrorItem(ex.Message));
                }
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public async Task<Result> RemoveAsync(int menuId)
        {
            var result = new Result();

            try
            {
                var page = await GetByIdAsync(menuId);

                await _menuDataAccess.SaveAsync(page, DataAccessActions.Delete);

            }
            catch (Exception ex)
            {
                _logger.LogError($"RemoveAsync error: {ex.Message}, stack: {ex.StackTrace}");
                result.Errors.Add(new ErrorItem(ex.Message));
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public async Task<Menu> GetByIdAsync(int menuId)
        {
            var result = await GetByAsync(null, menuId, true, null, 0, 1);

            if (result.Count > 0)
                return result[0];

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="id"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public async Task<List<Menu>> GetByAsync(int? siteId = null, int? id = null, bool? isActive = null, string name = null, int from = 0, int to = 10)
        {
            var menus = await _menuDataAccess.GetByAsync(siteId, id, isActive, name, from, to);

            foreach (var menu in menus)
            {
                menu.Items = await _menuDataAccess.GetItemsByAsync(menu.Id, null);
            }

            return menus;
        }
        
        public async Task<Result> SaveItemAsync(MenuItem menuItem, string username)
        {
            menuItem.Audit(username);

            var result = _itemValidator.Validate(menuItem);

            if (result.IsValid)
            {
                try
                {
                    menuItem.Id = result.NewId = await _menuDataAccess.SaveItemAsync(menuItem, menuItem.IsNew ? DataAccessActions.Insert : DataAccessActions.Update);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"SaveItemAsync error: {ex.Message}, stack: {ex.StackTrace}");
                    result.Errors.Add(new ErrorItem(ex.Message));
                }
            }

            return result;
        }

        public async Task<Result> RemoveItemAsync(int menuItemId)
        {
            var result = new Result();

            try
            {
                var menuItem = await GetItemByIdAsync(menuItemId);

                await _menuDataAccess.SaveItemAsync(menuItem, DataAccessActions.Delete);

            }
            catch (Exception ex)
            {
                _logger.LogError($"RemoveItemAsync error: {ex.Message}, stack: {ex.StackTrace}");
                result.Errors.Add(new ErrorItem(ex.Message));
            }

            return result;
        }

        public async Task<List<MenuItem>> GetItemsAsync(int? menuId = null, int? id = null)
        {
            return await _menuDataAccess.GetItemsByAsync(menuId, id);
        }

        public async Task<MenuItem> GetItemByIdAsync(int menuItemId)
        {
            var list = await GetItemsAsync(id: menuItemId);

            if (list.Count == 1)
                return list[0];
           
            return null;
        }


        public async Task<List<Menu>> GetBySiteIdAsync(int siteId)
        {
            if (_menus.ContainsKey(siteId))
                return _menus[siteId];

            var result = await GetByAsync(siteId, null, true, null, 0, 10);

            if (result.Count > 0)
                return result;

            return null;
        }

        public async Task LoadMenus()
        {
            var menus = await GetByAsync(null, null, null, null, 0, 1000);

            foreach (var menu in menus)
            {
                if (!_menus.ContainsKey(menu.SiteId))
                {
                    var list = new List<Menu>() { menu };
                    _menus.AddOrUpdate(menu.SiteId, list, (key, oldValue) => list);
                }
                else
                {
                    _menus[menu.SiteId].Add(menu);
                }
            }
        }
    }
}
