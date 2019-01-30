using GetCms.Models.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GetCms.Samples.WebApp.Services
{
    public class CacheLoadService : IHostedService
    {
        private readonly ISiteService _siteService;
        private readonly IPageService _pageService;
        private readonly IMenusService _menuService;
        public CacheLoadService(ISiteService siteService, 
            IPageService pageService, 
            IMenusService menuService)
        {
            _siteService = siteService;
            _pageService = pageService;
            _menuService = menuService;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _siteService.LoadSites();
            await _pageService.LoadPages();
            await _menuService.LoadMenus();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
