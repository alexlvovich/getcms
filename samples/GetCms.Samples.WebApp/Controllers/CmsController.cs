using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetCms.Models;
using GetCms.Models.Services;
using GetCms.Samples.WebApp.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GetCms.Samples.WebApp.Controllers
{
    public class CmsController : Controller
    {
        private readonly ILogger _logger;
        private readonly ISiteService _siteService;
        private readonly IPageService _pageService;
        private readonly IMenusService _menusService;
        public CmsController(ILoggerFactory logginFactory, ISiteService siteService,
            IPageService pageService, IMenusService menusService)
        {
            _logger = logginFactory.CreateLogger<CmsController>();
            _siteService = siteService;
            _pageService = pageService;
            _menusService = menusService;
        }
        // GET: /<controller>/
        public async Task<ActionResult<WebAppModelItem<Page>>> Index(string lang, string pagePath)
        {
            _logger.LogDebug("Requesting '{0}' page", pagePath);

            Site oSite = await _siteService.GetDefaultSiteAsync();

            if (lang.Length != 2) return null;

            // magic
            pagePath = pagePath.ToLower();

            // get page
            Page page = await _pageService.GetBySlug(pagePath, oSite.Id);

            page.MasterPage = await _pageService.GetByIdAync(page.MasterPageId.Value);

            // menu
            var menus = await _menusService.GetBySiteIdAsync(oSite.Id);

            return View(new WebAppModelItem<Page>
            {
                Item = page,
                Site = oSite,
                Menus = menus.ToDictionary(x => x.Name, x => x)
            });
        }
    }
}
