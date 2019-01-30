using GetCms.Models;
using GetCms.Models.DataAccess;
using GetCms.Models.Enums;
using GetCms.Models.Services;
using GetCms.Services.Cms;
using GetCms.Services.Validation;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GetCms.Services.Tests
{
    public class MenuServiceTests : BaseServiceTests
    {
        private readonly IMenusService _menusService;
        private int _menuCounter = 1;
        private List<Menu> _menus = new List<Menu>();
        private readonly Mock<IMenusDataAccess> _menuDataAccess = new Mock<IMenusDataAccess>();

        public MenuServiceTests()
        {
            _menuDataAccess.Setup(m => m.SaveAsync(It.IsAny<Menu>(), It.IsAny<DataAccessActions>()))
            .Returns(Task.FromResult(_menuCounter)) //<-- returning the input value from task.
            .Callback(
            (Menu menu, DataAccessActions action) =>
            {
                if (action == DataAccessActions.Insert)
                {
                    _menus.Add(menu);
                    _menuCounter++;
                }
                else if (action == DataAccessActions.Update)
                {

                }
                else
                {
                    // delete
                }
            });


            _menusService = new MenusService(new LoggerFactory(),
               new MenuValidator(),
               new MenuItemValidator(),
               _menuDataAccess.Object);
        }


        [Fact]
        public async Task CreateMenu()
        {
            var menu = new Menu()
            {
                Name = "TopMenu",
                SiteId = SITE_ID,
                Items = new List<MenuItem>()
                {
                    new MenuItem() { Link = "/home", Text = "Home" },
                    new MenuItem() { Link = "/about", Text = "About" }
                }
            };

            var result = await _menusService.SaveAsync(menu, TEST_USER);

            Assert.NotNull(result);

            Assert.True(result.Succeeded);
            Assert.True(result.NewId > 0);
        }
    }
}
