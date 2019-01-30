using GetCms.Models;
using GetCms.Models.Cms.Enums;
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
    public class MetasServiceIntegrationTests : BaseIntegrationTests
    {
        const string METAKEY = "keywords";
        const string UPDATEUSER = "updater@test.com";
        private int _metaCounter = 1;
        private List<MetaData> _datas = new List<MetaData>();
        public MetasServiceIntegrationTests()
        {
        }


        [Fact]
        public async Task CreateMetaData()
        {
            var page = await GetRandomPage();

            var m = new MetaData()
            {
                Key = METAKEY,
                SiteId = page.SiteId,
                Value = "test keyword",
                Type = MetaDataTypes.Page,
                ItemId = page.Id
            };

            var result = await _metasService.SaveAsync(m, TEST_USER);

            Assert.NotNull(result);

            Assert.True(result.Succeeded);
            Assert.True(result.NewId > 0);
        }


        [Fact]
        public async Task UpdateMetaData()
        {
            var page = await GetRandomPage();

            var m = new MetaData()
            {
                Key = METAKEY,
                SiteId = page.SiteId,
                Value = "test keyword",
                Type = MetaDataTypes.Page,
                ItemId = page.Id
            };

            var result = await _metasService.SaveAsync(m, TEST_USER);

            Assert.NotNull(result);

            Assert.True(result.Succeeded);
            Assert.True(result.NewId > 0);

            var metaData = await _metasService.GetAsync(page.Id, page.SiteId);

            Assert.True(metaData.Count == 1);

            metaData[METAKEY].Value = "upadated value.";

            result = await _metasService.SaveAsync(metaData[METAKEY], UPDATEUSER);

            Assert.True(result.Succeeded);

            var metaDataUpdated = await _metasService.GetAsync(page.Id, page.SiteId);

            Assert.True(metaDataUpdated.Count == 1);

            Assert.True(metaDataUpdated[METAKEY].Value == metaData[METAKEY].Value);
            Assert.True(metaDataUpdated[METAKEY].ModifiedBy == UPDATEUSER);
        }

        [Fact]
        public async Task CreateMetaDataWithMissingSiteId()
        {
            var m = new MetaData()
            {
                Key = METAKEY,
                SiteId = 0,
                Value = "test keyword"
            };

            var result = await _metasService.SaveAsync(m, TEST_USER);

            Assert.NotNull(result);

            Assert.False(result.Succeeded);
            Assert.True(result.NewId == 0);
            Assert.True(result.ValiationErrors.Count > 0);
        }

        [Fact]
        public async Task CreateMetaDataWithMissingType()
        {
            var site = await GetSite();

            var m = new MetaData()
            {
                Key = METAKEY,
                SiteId = site.Id,
                Value = "test keyword",
                Type = MetaDataTypes.NotSet
            };

            var result = await _metasService.SaveAsync(m, TEST_USER);

            Assert.NotNull(result);

            Assert.False(result.Succeeded);
            Assert.True(result.NewId == 0);
            Assert.True(result.ValiationErrors.Count > 0);
        }
    }
}
