using GetCms.Models;
using GetCms.Models.DataAccess;
using GetCms.Models.Enums;
using GetCms.Models.Enums.Messaging;
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
    public class ContentServiceTests : BaseServiceTests
    {
        private int _contentCounter = 1;
        private List<Content> _contents = new List<Content>();
        private readonly IContentService _contentService;

        private readonly Mock<IContentsDataAccess> _contentDataAccess = new Mock<IContentsDataAccess>();
        public ContentServiceTests()
        {
            _contentDataAccess.Setup(m => m.SaveAsync(It.IsAny<Content>(), It.IsAny<DataAccessActions>()))
            .Returns(Task.FromResult(_contentCounter)) //<-- returning the input value from task.
            .Callback(
            (Content c, DataAccessActions action) =>
            {
                if (action == DataAccessActions.Insert)
                {
                    _contents.Add(c);
                    _contentCounter++;
                }
                else if (action == DataAccessActions.Update)
                {

                }
                else
                {
                    // delete
                }
                
            });


            _contentService = new ContentService(new LoggerFactory(),
               _contentDataAccess.Object,
               new ContentValidator());
        }

        [Fact]
        public async Task CreateContent()
        {
            var page = Get();

            var result = await _contentService.SaveAsync(page, TEST_USER);

            Assert.NotNull(result);

            Assert.True(result.Succeeded);
            Assert.True(result.NewId > 0);
        }

        [Fact]
        public async Task CreateContentWithMissingSiteId()
        {
            var page = Get();

            page.SiteId = 0;

            var result = await _contentService.SaveAsync(page, TEST_USER);

            Assert.NotNull(result);

            Assert.False(result.Succeeded);
            Assert.True(result.NewId == 0);
            Assert.True(result.ValiationErrors.Count > 0);
        }

        [Fact]
        public async Task CreateContentWithMissingName()
        {
            var page = Get();

            page.Name = string.Empty;

            var result = await _contentService.SaveAsync(page, TEST_USER);

            Assert.NotNull(result);

            Assert.False(result.Succeeded);
            Assert.True(result.NewId == 0);
            Assert.True(result.ValiationErrors.Count > 0);
        }

        [Fact]
        public async Task CreateEmailTempalteTest()
        {
            var emailTmpl = new MessagingTemplate()
            {
                Body = "<html>{##USERNAME##}, {##PASSWORD##}</html>",
                Name = "template test",
                Type = ContentTypes.EmailTemplate,
                SiteId = SITE_ID,
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
        
        private Content Get()
        {
            string name = $"content-{DateTime.Now.Ticks}";
            var content = new Content()
            {
                Name = name,
                SiteId = SITE_ID,
                IsActive = true,
                Body = "test body",
                Type = ContentTypes.PageComponent
            };

            return content;
        }
    }
}
