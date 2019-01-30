using GetCms.Models;
using GetCms.Models.Cms.Enums;
using GetCms.Models.Enums;
using GetCms.Models.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GetCms.Samples.WebApp.Services
{
    public class SeedDataService : IHostedService
    {
        const string USER = "admin@domain.com";
        private readonly ISiteService _siteService;
        private readonly IPageService _pageService;
        private readonly IContentService _contentService;
        private readonly IMenusService _menuService;
        private readonly ILogger _logger;

        private IConfiguration _configuration;

        Dictionary<string, MetaData> metaDatas = new Dictionary<string, MetaData>();
        public SeedDataService(ILoggerFactory logginFactory, 
            ISiteService siteService, 
            IPageService pageService, 
            IContentService contentService, 
            IConfiguration configuration, 
            IMenusService menuService)
        {
            _siteService = siteService;
            _pageService = pageService;
            _contentService = contentService;
            _configuration = configuration;
            _menuService = menuService;
            _logger = logginFactory.CreateLogger<SeedDataService>();
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var toRun = _configuration.GetValue<bool>("General:SeedData");

            if (!toRun) return;

            metaDatas.Add("description", new MetaData() { Value = "Demo description", Key = "description" });
            try
            {
                // create site
                var site = new Site() { Name = "Demo", Host = "www.domain.com", Language = Languages.English, IsActive = true };
                var siteCreationResult = await _siteService.SaveAsync(site, USER);

                if (!siteCreationResult.Succeeded)
                    throw new Exception("Site was not created");

                // create page componet contact form
                var contactForm = new Content() { Name = "ContactForm", SiteId = siteCreationResult.NewId, Type = ContentTypes.PageComponent, IsActive = true };
                var contentCreationResult = await _contentService.SaveAsync(contactForm, USER);
                if (!contentCreationResult.Succeeded)
                    throw new Exception("Content was not created");

                contactForm.Id = contentCreationResult.NewId;

                // create master page
                var masterPageCreationResult = await _pageService.SaveAsync(new Page() { Name = "Layout", Slug = "layout", SiteId = siteCreationResult.NewId, PageType = PageTypes.MasterPage, IsActive = true, MetaData = new Dictionary<string, MetaData>() }, USER);
                if (!masterPageCreationResult.Succeeded)
                    throw new Exception("Master page was not created");

                // create menu 
                var menuCreationResult = await _menuService.SaveAsync(new Menu() { SiteId = siteCreationResult.NewId, Name="TopMenu", IsActive=true, Description = "Top menu" }, USER);
                if (!menuCreationResult.Succeeded)
                    throw new Exception("Menu was not created");


                // create pages home, about, contactform 
                var pages = new List<Page>()
                {
                    new Page() { MasterPageId = masterPageCreationResult.NewId, Name = "Home", Slug= "home", SiteId = siteCreationResult.NewId, Title = "Demo homepage", IsActive = true, PageType = PageTypes.ContentPage,
                        Contents = new List<Content>() {
                            new Content() { Name= $"content-home", Body = "<p>Weclome to demo home page</p>", Type = ContentTypes.InlineHTML, IsActive = true, SiteId = siteCreationResult.NewId}
                        },
                        MetaData = metaDatas
                    },
                    new Page() { MasterPageId = masterPageCreationResult.NewId, Name = "About", Slug= "about", SiteId = siteCreationResult.NewId, Title = "About", IsActive = true, PageType = PageTypes.ContentPage,
                        Contents = new List<Content>() {
                            new Content() { Name= $"content-about", Body = "<p>About us</p>", Type = ContentTypes.InlineHTML, IsActive = true, SiteId = siteCreationResult.NewId}
                        },
                        MetaData = metaDatas
                    },
                    new Page() { MasterPageId = masterPageCreationResult.NewId, Name = "Contactus", Slug= "contact", SiteId = siteCreationResult.NewId, Title = "Contact us", IsActive = true, PageType = PageTypes.ContentPage,
                        Contents = new List<Content>() {
                            contactForm
                        },
                        MetaData = metaDatas
                    }
                };

                byte i = 1;
                foreach (var page in pages)
                {
                    var pageCreationResult = await _pageService.SaveAsync(page, USER);
                    if (!pageCreationResult.Succeeded)
                        throw new Exception("Page not created");

                    var menuItem = new MenuItem() {
                        Text = page.Name,
                        Link = $"/{site.Language.Code}/{page.Slug}",
                        MenuId = menuCreationResult.NewId,
                        Alt = page.Name,
                        IsActive = true,
                        Order = i
                    };

                    var meniItemCreationResult = await _menuService.SaveItemAsync(menuItem, USER);
                    if (!meniItemCreationResult.Succeeded)
                        throw new Exception("Menu item was not created");
                    i++;
                }


                // create menu
            }
            catch (Exception ex)
            {
                _logger.LogError($"Seeding data error: {ex.Message}, stack: {ex.StackTrace}");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // noop
            return Task.CompletedTask;
        }
    }
}
