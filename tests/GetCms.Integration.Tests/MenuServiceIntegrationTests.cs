using GetCms.Models;
using GetCms.Models.DataAccess;
using GetCms.Models.Enums;
using GetCms.Models.Services;
using GetCms.Services.Cms;
using GetCms.Services.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GetCms.Integration.Tests
{
    public class MenuServiceIntegrationTests : BaseIntegrationTests
    {
        private readonly IMenusService _menusService;
        private int _menuCounter = 1;
        private List<Menu> _menus = new List<Menu>();
        

        public MenuServiceIntegrationTests()
        {
            
            _menusService = new MenusService(new LoggerFactory(),
               new MenuValidator(),
               new MenuItemValidator(),
               _menuDataAccess);
        }


        [Fact]
        public async Task CreateMenu()
        {
            var site = await GetSite();

            var menu = new Menu()
            {
                Name = "TopMenu",
                SiteId = site.Id,
                Items = new List<MenuItem>()
                {
                    new MenuItem() { Link = "/home", Text = "Home" },
                    new MenuItem() { Link = "/about", Text = "About" }
                },
                IsActive = true
            };

            var result = await _menusService.SaveAsync(menu, TEST_USER);

            Assert.NotNull(result);

            Assert.True(result.Succeeded);
            Assert.True(result.NewId > 0);
        }
    }
}
