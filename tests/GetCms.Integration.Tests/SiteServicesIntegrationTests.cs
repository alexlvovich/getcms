using GetCms.Models;
using GetCms.Models.DataAccess;
using GetCms.Models.Enums;
using GetCms.Models.Services;
using GetCms.Services;
using GetCms.Services.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GetCms.Integration.Tests
{ 
    public class SiteServiceTests : BaseIntegrationTests
    {
        
        private int _siteCounter = 1;
        private List<Site> _sites = new List<Site>();
       


        public SiteServiceTests()
        {
           
        }


        private List<Site> GetSites()
        {
            return _sites.OrderByDescending(s => s.CreatedOn).ToList();
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

            Assert.NotNull(newSite);

            Assert.True(newSite.Id == result.NewId);
        }


        [Fact]
        public async Task CreateNewSiteAndGetByName()
        {
            string userName = "dev@test.com";
            var site = new Site()
            {
                Name = $"Test-site-{DateTime.Now.Ticks}",
                Language = Languages.English,
                CreatedBy = userName,
                Host = "www.host.com"
            };

            var result = await _siteService.SaveAsync(site, userName);

            Assert.NotNull(result);

            Assert.True(result.Succeeded);
            Assert.True(result.NewId > 0);

            var getByResult = await _siteService.GetByAsync(null, null, site.Name, 0, 1);

            Assert.NotNull(getByResult);
            Assert.True(getByResult.Count == 1);
            Assert.True(getByResult[0].Id == result.NewId);
        }
    }
}
