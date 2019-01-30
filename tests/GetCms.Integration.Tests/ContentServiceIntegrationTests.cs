using GetCms.Models;
using GetCms.Models.DataAccess;
using GetCms.Models.Enums;
using GetCms.Models.Enums.Messaging;
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
    public class ContentServiceIntegrationTests : BaseIntegrationTests
    {
        private int _contentCounter = 1;
        private List<Content> _contents = new List<Content>();

        public ContentServiceIntegrationTests()
        {
            
        }

        [Fact]
        public async Task CreateContent()
        {
            var content = await GetContent();

            var result = await _contentService.SaveAsync(content, TEST_USER);

            Assert.NotNull(result);

            Assert.True(result.Succeeded);
            Assert.True(result.NewId > 0);
        }

        [Fact]
        public async Task CreateContentWithMissingSiteId()
        {
            var content = await GetContent();

            content.SiteId = 0;

            var result = await _contentService.SaveAsync(content, TEST_USER);

            Assert.NotNull(result);

            Assert.False(result.Succeeded);
            Assert.True(result.NewId == 0);
            Assert.True(result.ValiationErrors.Count > 0);
        }

        [Fact]
        public async Task CreateContentWithMissingName()
        {
            var content = await GetContent();

            content.Name = string.Empty;

            var result = await _contentService.SaveAsync(content, TEST_USER);

            Assert.NotNull(result);

            Assert.False(result.Succeeded);
            Assert.True(result.NewId == 0);
            Assert.True(result.ValiationErrors.Count > 0);
        }

        [Fact]
        public async Task CreateEmailTempalteTest()
        {
            var site = await GetSite();

            var emailTmpl = new MessagingTemplate()
            {
                Body = "<html>{##USERNAME##}, {##PASSWORD##}</html>",
                Name = "template test",
                Type = ContentTypes.EmailTemplate,
                SiteId = site.Id,
                Subject = "Register",
                TemplateType = TemplateTypes.UserActivation,
                Target = TargetTypes.Email
            };


            var result = await _contentService.SaveAsync(emailTmpl, TEST_USER);

            Assert.NotNull(result);

            Assert.True(result.Succeeded);
            Assert.True(result.NewId > 0);
            Assert.True(result.ValiationErrors.Count == 0);

        }
        
        private async Task<Content> GetContent()
        {
            var site = await GetSite();

            string name = $"content-{DateTime.Now.Ticks}";
            var content = new Content()
            {
                Name = name,
                SiteId = site.Id,
                IsActive = true,
                Body = "test body",
                Type = ContentTypes.PageComponent
            };

            return content;
        }
    }
}
