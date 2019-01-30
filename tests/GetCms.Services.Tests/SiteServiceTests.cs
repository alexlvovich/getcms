using GetCms.Models;
using GetCms.Models.DataAccess;
using GetCms.Models.Enums;
using GetCms.Models.Services;
using GetCms.Services.Validation;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GetCms.Services.Tests
{
    public class SiteServicesIntegrationTests
    {
        private readonly ISiteService _siteService;
        private int _siteCounter = 1;
        private List<Site> _sites = new List<Site>();
        private readonly Mock<ISitesDataAccess> _sitesDataAccess = new Mock<ISitesDataAccess>();


        public SiteServicesIntegrationTests()
        {

            _sitesDataAccess.Setup(m => m.GetByAsync(It.IsAny<int>(), null, null, null, null, null, 0, 1))
               .Returns((async () => GetSites()));

            _sitesDataAccess.Setup(m => m.SaveAsync(It.IsAny<Site>(), DataAccessActions.Insert))
              .Returns(Task.FromResult(_siteCounter)) //<-- returning the input value from task.
              .Callback(
              (Site site, DataAccessActions action) =>
              {
                  _sites.Add(site);
                  _siteCounter++;
              });



            _siteService = new SitesService(new LoggerFactory(),
                _sitesDataAccess.Object,
                new SiteValidator());
        }


        private List<Site> GetSites()
        {
            return _sites.OrderByDescending(s => s.CreatedOn).ToList();
        }

        [Fact]
        public async Task CreateNewSite()
        {
            string userName = "dev@test.com";
            var site = new Site()
            {
                Name = "Test website",
                Language = Languages.English,
                CreatedBy = userName,
                Host = "www.host.com"
            };

            var result = await _siteService.SaveAsync(site, userName);

            Assert.NotNull(result);

            Assert.True(result.Succeeded);
            Assert.True(result.NewId > 0);
        }


        [Fact]
        public async Task CreateNewSiteMissingName()
        {
            string userName = "dev@test.com";
            var site = new Site()
            {
                Name = string.Empty,
                Language = Languages.English,
                CreatedBy = userName,
                Host = "www.host.com"
            };

            var result = await _siteService.SaveAsync(site, userName);

            Assert.NotNull(result);

            Assert.False(result.Succeeded);
            Assert.True(result.NewId == 0);
            Assert.True(result.ValiationErrors.Count > 0);
        }

        [Fact]
        public async Task CreateNewSiteMissingHost()
        {
            string userName = "dev@test.com";
            var site = new Site()
            {
                Name = "Test website",
                Language = Languages.English,
                CreatedBy = userName
            };

            var result = await _siteService.SaveAsync(site, userName);

            Assert.NotNull(result);

            Assert.False(result.Succeeded);
            Assert.True(result.NewId == 0);
            Assert.True(result.ValiationErrors.Count > 0);
        }

        [Fact]
        public async Task CreateNewSiteMissingAuthor()
        {
            var site = new Site()
            {
                Name = "New site",
                Language = Languages.English,
                Host = "www.host.com"
            };

            var result = await _siteService.SaveAsync(site, string.Empty);

            Assert.NotNull(result);

            Assert.False(result.Succeeded);
            Assert.True(result.NewId == 0);
            Assert.True(result.ValiationErrors.Count > 0);
        }

        [Fact]
        public async Task CreateNewSiteAndGetById()
        {
            string userName = "dev@test.com";
            var site = new Site()
            {
                Name = "Test website",
                Language = Languages.English,
                CreatedBy = userName,
                Host = "www.host.com"
            };

            var result = await _siteService.SaveAsync(site, userName);

            Assert.NotNull(result);

            Assert.True(result.Succeeded);
            Assert.True(result.NewId > 0);

            var newSite = await _siteService.GetByIdAsync(result.NewId);

            Assert.NotNull(result);

            Assert.True(newSite.Id == result.NewId);
        }

    }
}
