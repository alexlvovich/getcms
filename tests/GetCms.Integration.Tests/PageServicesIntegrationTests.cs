using GetCms.Models;
using GetCms.Models.Cms.Enums;
using GetCms.Models.DataAccess;
using GetCms.Models.Enums;
using GetCms.Models.Services;
using GetCms.Services.Cms;
using GetCms.Services.Validation;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GetCms.Integration.Tests
{
    public class PageServicesIntegrationTests : BaseIntegrationTests
    {
        private int _pageCounter = 1;
        private List<Page> _pages = new List<Page>();
        
        public PageServicesIntegrationTests()
        {
     
        }


        [Fact]
        public async Task CreatePage()
        {
            var page = await GetPage();

            var result = await _pageService.SaveAsync(page, TEST_USER);

            Assert.NotNull(result);

            Assert.True(result.Succeeded);
            Assert.True(result.NewId > 0);
        }

        [Fact]
        public async Task CreatePageWithMissingSiteId()
        {
            var page = await GetPage();

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
            var page = await GetPage();

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
            var page = await GetPage();

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
            var page = await GetPage();

            var result = await _pageService.SaveAsync(page, TEST_USER);

            Assert.NotNull(result);

            Assert.True(result.Succeeded);
            Assert.True(result.NewId > 0);

            string str = JsonConvert.SerializeObject(page);

            var dpage = JsonConvert.DeserializeObject<Page>(str);

            dpage.Id = 0;
            dpage.Contents[0].Id = 0;

            dpage.MetaData["keywords"].Id = 0;
            dpage.MetaData["description"].Id = 0;

            result = await _pageService.SaveAsync(dpage, TEST_USER);

            Assert.NotNull(result);

            Assert.False(result.Succeeded);
            Assert.True(result.NewId == 0);
            Assert.True(result.ValiationErrors.Count > 0);
        }

        [Fact]
        public async Task CreateChildPage()
        {
            var p = await GetRandomPage();

            var page = await GetPage();

            page.SiteId = p.SiteId;
            page.ParentId = p.Id;

            var result = await _pageService.SaveAsync(page, TEST_USER);

            Assert.NotNull(result);

            Assert.True(result.Succeeded);
            Assert.True(result.NewId > 0);

            var r = await _pageService.GetByAsync(p.SiteId, null, null, null, null, null, p.Id, null, 0, 10);

            Assert.NotNull(r);
            Assert.True(r.Total > 0);

            foreach (var pg in r.List)
            {
                Assert.True(pg.ParentId == p.Id);
            }

        }

        [Fact]
        public async Task GetPublishedPage()
        {
            var p = await GetRandomPage();

            p.PublishedBy = TEST_USER;
            p.PublishedOn = DateTime.Now;


            var result = await _pageService.SaveAsync(p, TEST_USER);

            Assert.NotNull(result);

            Assert.True(result.Succeeded);
            Assert.True(result.NewId > 0);

            var r = await _pageService.GetByAsync(p.SiteId, p.Id, null, null, true, null, null, null, 0, 1);

            Assert.NotNull(r);
            Assert.True(r.Total > 0);
        }

        [Fact]
        public async Task GetNotPublishedPage()
        {
            var p = await GetRandomPage();
            
            var result = await _pageService.SaveAsync(p, TEST_USER);

            Assert.NotNull(result);

            Assert.True(result.Succeeded);
            Assert.True(result.NewId > 0);

            var r = await _pageService.GetByAsync(p.SiteId, p.Id, null, null, false, null, null, null, 0, 1);

            Assert.NotNull(r);
            Assert.True(r.Total > 0);
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

        private async Task<Page> GetPage()
        {
            Dictionary<string, MetaData> metaDatas = new Dictionary<string, MetaData>();

            var site = await GetSite();

            metaDatas.Add("keywords", new MetaData() { Value = "keywords test", Key = "keywords" });
            metaDatas.Add("description", new MetaData() { Value = "description test", Key = "description" });

            string name = $"Test-{DateTime.Now.Ticks}";
            var page = new Page()
            {
                Name = name,
                SiteId = site.Id,
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
