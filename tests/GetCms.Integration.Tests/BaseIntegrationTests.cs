using GetCms.DataAccess.SqlServer;
using GetCms.Models;
using GetCms.Models.DataAccess;
using GetCms.Models.Services;
using GetCms.Services;
using GetCms.Services.Cms;
using GetCms.Services.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GetCms.Integration.Tests
{
    public abstract class BaseIntegrationTests
    {
        internal const string TEST_USER = "dev@test.com";
        internal const int SITE_ID = 1;

        internal readonly ISitesDataAccess _sitesDataAccess;
        internal readonly IPagesDataAccess _pageDataAccess;
        internal readonly IContentsDataAccess _contentDataAccess;
        internal readonly IMetasDataAccess _metasDataAccess;
        internal readonly IMenusDataAccess _menuDataAccess;

        internal readonly ISiteService _siteService;
        internal readonly IPageService _pageService;
        internal readonly IMetasService _metasService;
        internal readonly IContentService _contentService;



        private IConfiguration _configuration { get; set; }
        public BaseIntegrationTests()
        {
            Init();

            var loggerFactory = new LoggerFactory();

            // data access init 
            _sitesDataAccess = new SitesDataAccess(_configuration.GetConnectionString("default"), loggerFactory);
            _pageDataAccess = new PagesDataAccess(_configuration.GetConnectionString("default"), loggerFactory);
            _contentDataAccess = new ContentsDataAccess(_configuration.GetConnectionString("default"), loggerFactory);
            _metasDataAccess = new MetasDataAccess(_configuration.GetConnectionString("default"), loggerFactory);
            _menuDataAccess = new MenusDataAccess(_configuration.GetConnectionString("default"), loggerFactory);


            _metasService = new MetasService(loggerFactory, _metasDataAccess, new MetadataValidator());
            _contentService = new ContentService(loggerFactory, _contentDataAccess, new ContentValidator());
            _siteService = new SitesService(loggerFactory,
               _sitesDataAccess,
               new SiteValidator());

            _pageService = new PagesService(loggerFactory, 
                _pageDataAccess, _metasService, _contentService, new PageValidator(_pageDataAccess), _siteService);


            
        }

        
        public void Init()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("test-settings.json")
                .Build();
        }

        public async Task<Site> GetSite()
        {
            var sites = await _sitesDataAccess.GetByAsync(null, null, null, string.Empty, string.Empty, string.Empty, 0, 10);

            var rnd = new Random();

            int i = rnd.Next(0, sites.Count-1);

            return sites[i];
        }

        public async Task<Page> GetRandomPage()
        {
            var pages = await _pageDataAccess.GetByAsync(null, null, string.Empty, string.Empty,  0, 10);

            var rnd = new Random();

            int i = rnd.Next(0, pages.List.Count);

            return pages.List[i];
        }

    }
}
