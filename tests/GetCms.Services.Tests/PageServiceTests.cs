using GetCms.Models;
using GetCms.Models.Cms.Enums;
using GetCms.Models.DataAccess;
using GetCms.Models.Enums;
using GetCms.Models.General;
using GetCms.Models.Services;
using GetCms.Services.Cms;
using GetCms.Services.Validation;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GetCms.Services.Tests
{
    public class PageServiceTests : BaseServiceTests
    {
        private readonly IPageService _pageService;
        private int _pageCounter = 1;
        private readonly List<Page> _pages = new List<Page>();
        private readonly Mock<IPagesDataAccess> _pagesDataAccess = new Mock<IPagesDataAccess>();
        private readonly Mock<IMetasService> _metaService = new Mock<IMetasService>();
        private readonly Mock<IContentService> _contentService = new Mock<IContentService>();
        private readonly Mock<ISiteService> _siteService = new Mock<ISiteService>();
        public PageServiceTests()
        {
            _pagesDataAccess.Setup(m => m.SaveAsync(It.IsAny<Page>(), DataAccessActions.Insert))
            .Returns(Task.FromResult(_pageCounter)) //<-- returning the input value from task.
            .Callback(
            (Page page, DataAccessActions action) =>
            {
                _pages.Add(page);
                _pageCounter++;
            });

            _pagesDataAccess.Setup(m => m.GetByAsync(It.IsAny<int>(), null, null, It.IsAny<string>(), null, null, null, null, 0, 1))
            .Returns(async (int? siteId, int? id, string name, string slug, bool? published, bool? active, int? parentId, byte? type, int from, int to) => {

                var list = _pages.Where(p => p.Slug == slug && p.SiteId == siteId.Value).ToList();

                return new PagedResults<Page>() { List = list, Total = list.Count };
            });

            _pageService = new PagesService(new LoggerFactory(),
                _pagesDataAccess.Object,
                _metaService.Object,
                _contentService.Object,
                new PageValidator(_pagesDataAccess.Object), _siteService.Object);


            _contentService.Setup(m => m.SaveAsync(It.IsAny<Content>(), It.IsAny<string>())).Returns(Task.FromResult(new Result() { NewId = 1 }));
            _contentService.Setup(m => m.MapToPage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(Task.FromResult(new Result() { NewId = 1 }));
        }


        [Fact]
        public async Task CreatePage()
        {
            var page = GetPage();

            var result = await _pageService.SaveAsync(page, TEST_USER);

            Assert.NotNull(result);

            Assert.True(result.Succeeded);
            Assert.True(result.NewId > 0);
        }

        [Fact]
        public async Task CreatePageWithMissingSiteId()
        {
            var page = GetPage();

            page.SiteId = 0;

            var result = await _pageService.SaveAsync(page, TEST_USER);

            Assert.NotNull(result);

            Assert.False(result.Succeeded);
            Assert.True(result.NewId == 0);
            Assert.True(result.ValiationErrors.Count > 0);
        }

        [Fact]
        public async Task CreatePageWithMissingName()
        {
            var page = GetPage();

            page.Name = string.Empty;

            var result = await _pageService.SaveAsync(page, TEST_USER);

            Assert.NotNull(result);

            Assert.False(result.Succeeded);
            Assert.True(result.NewId == 0);
            Assert.True(result.ValiationErrors.Count > 0);
        }

        [Fact]
        public async Task CreatePageWithMissingSlug()
        {
            var page = GetPage();

            page.Slug = string.Empty;

            var result = await _pageService.SaveAsync(page, TEST_USER);

            Assert.NotNull(result);

            Assert.False(result.Succeeded);
            Assert.True(result.NewId == 0);
            Assert.True(result.ValiationErrors.Count > 0);
        }

        [Fact]
        public async Task CreateDuplicatePageTest()
        {
            var page = GetPage();

            var result = await _pageService.SaveAsync(page, TEST_USER);

            Assert.NotNull(result);

            Assert.True(result.Succeeded);
            Assert.True(result.NewId > 0);


            var dpage = new Page()
            {
                Name = page.Name,
                SiteId = SITE_ID,
                PageType = PageTypes.ContentPage,
                IsActive = true,
                Slug = page.Name.ToLower()
            };

            result = await _pageService.SaveAsync(dpage, TEST_USER);

            Assert.NotNull(result);

            Assert.False(result.Succeeded);
            Assert.True(result.NewId == 0);
            Assert.True(result.ValiationErrors.Count > 0);
        }



        //[Fact]
        //public void UpdatePageTest()
        //{
        //    var controller = GetController();

        //    var page = GetPage();

        //    var res = controller.Post(page);

        //    Assert.IsNotNull(res, "call failed");
        //    Assert.IsTrue(res.Result.StatusCode == HttpStatusCode.Created);

        //    var pages = controller.Get(1, "Test", 0, false, 0, 1);

        //    Assert.IsTrue(pages.Count == 1, "get by query doesn't work");

        //    var p = pages[0];

        //    p.Name = $"{p.Name}-change";

        //    res = controller.Put(p);

        //    Assert.IsNotNull(res, "call failed");
        //    Assert.IsTrue(res.Result.StatusCode == HttpStatusCode.OK);

        //    var updatedPage = controller.Get(0, string.Empty, p.PageId, false).First();

        //    Assert.IsTrue(p.Name == updatedPage.Name, "update was not saved");
        //}

        //[Fact]
        //public void PublishPageTest()
        //{
        //    var controller = GetController();


        //    Thread.CurrentPrincipal = new GenericPrincipal
        //    (
        //       new GenericIdentity(TESTUSER),
        //       new[] { "User" }
        //    );

        //    var page = GetPage();

        //    var res = controller.Post(page);

        //    Assert.IsNotNull(res, "call failed");
        //    Assert.IsTrue(res.Result.StatusCode == HttpStatusCode.Created);

        //    var pages = controller.Get(1, "Test", 0, false, 0, 1);

        //    Assert.IsTrue(pages.Count == 1, "get by query doesn't work");

        //    var p = pages[0];

        //    p.PublishedOn = DateTime.Now;
        //    p.PublishedBy = TESTUSER;

        //    res = controller.Put(p);

        //    Assert.IsNotNull(res, "call failed");
        //    Assert.IsTrue(res.Result.StatusCode == HttpStatusCode.OK);

        //    var updatedPage = controller.Get(0, string.Empty, p.PageId, false).First();

        //    Assert.IsTrue(updatedPage.PublishedOn.HasValue, "was not published");
        //}



        //[Fact]
        //public void DeletePageTest()
        //{
        //    var controller = GetController();

        //    Thread.CurrentPrincipal = new GenericPrincipal
        //       (
        //       new GenericIdentity(TESTUSER),
        //       new[] { "User" }
        //       );



        //    var page = GetPage();

        //    var res = controller.Post(page);

        //    Assert.IsNotNull(res, "call failed");
        //    Assert.IsTrue(res.Result.StatusCode == HttpStatusCode.Created);

        //    var pages = controller.Get(1, "Test", 0, false, 0, 1);

        //    Assert.IsTrue(pages.Count == 1, "get by query doesn't work");

        //    var p = pages[0];


        //    res = controller.Delete(p.PageId);

        //    Assert.IsNotNull(res, "call failed");
        //    Assert.IsTrue(res.Result.StatusCode == HttpStatusCode.OK);

        //    var updatedPage = controller.Get(0, string.Empty, p.PageId).FirstOrDefault();

        //    Assert.IsNull(updatedPage, "Page wasn't deleted");


        //}


        //[Fact]
        //public void GetPageByNameTest()
        //{
        //    var controller = GetController();



        //    var pages = controller.Get(1, string.Empty, 0, true, 0, 1);

        //    Assert.IsTrue(pages.Count == 1, "get by query doesn't work");


        //    var p = pages[0];


        //    var page = controller.Get(1, p.Name, 0);

        //    Assert.IsTrue(page.Count != 0, "can't find by name");



        //    page = controller.Get(1, p.Name.ToLower(), 0);

        //    Assert.IsTrue(page.Count != 0, "can't find by name to lower");

        //}

        private Page GetPage()
        {
            Dictionary<string, MetaData> metaDatas = new Dictionary<string, MetaData>
            {
                { "keywords", new MetaData() { Value = "keywords test", Key = "keywords" } },
                { "description", new MetaData() { Value = "description test", Key = "description" } }
            };

            string name = $"Test-{DateTime.Now.Ticks}";
            var page = new Page()
            {
                Name = name,
                SiteId = SITE_ID,
                PageType = PageTypes.ContentPage,
                IsActive = true,
                Slug = name.ToLower(),
                Contents = new List<Content>() { new Content(){
                    Name= $"content-{name}",
                    Body = name,
                    Type = ContentTypes.PageComponent,
                    IsActive = true,
                    SiteId = SITE_ID}},
                MetaData = metaDatas
            };
            
            return page;
        }



        //[Fact]
        //public void GetPageWithTextAndMetaData()
        //{
        //    var controller = GetController();



        //    var pages = controller.Get(1, string.Empty, 0, true, 0, 1);

        //    Assert.IsTrue(pages.Count == 1, "get by query doesn't work");


        //    Assert.IsTrue(pages[0].Contents.Count > 0, "no content for one page");


        //    Assert.IsTrue(pages[0].MetaData != null, "Meta data not saved");
        //}


        /// <summary>
        /// clean up
        /// </summary>
        //[Fact]
        //public void DeletePagesAsCleanUpTest()
        //{
        //    var controller = GetController();

        //    Thread.CurrentPrincipal = new GenericPrincipal
        //       (
        //       new GenericIdentity(TESTUSER),
        //       new[] { "User" }
        //       );

        //    var pages = controller.Get(1, "Test", 0, false, 0, 10);

        //    Assert.IsTrue(pages.Count > 0, "get by query doesn't work");

        //    foreach (var p in pages)
        //    {
        //        if (p.Name.StartsWith("Test"))
        //        {
        //            var res = controller.Delete(p.PageId);
        //            Assert.IsNotNull(res, "call failed");
        //            Assert.IsTrue(res.Result.StatusCode == HttpStatusCode.OK);
        //        }

        //    }

        //}
    }
}
