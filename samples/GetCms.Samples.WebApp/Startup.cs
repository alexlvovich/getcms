using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetCms.DataAccess.SqlServer;
using GetCms.Models;
using GetCms.Models.DataAccess;
using GetCms.Models.Services;
using GetCms.Models.Validation;
using GetCms.Samples.WebApp.Infrastructure;
using GetCms.Samples.WebApp.Infrastructure.Constrains;
using GetCms.Samples.WebApp.Services;
using GetCms.Services;
using GetCms.Services.Cms;
using GetCms.Services.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GetCms.Samples.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var loggerFactory = new LoggerFactory();
            services.AddSingleton<ILoggerFactory, LoggerFactory>();

            // dataaccess
            services.AddTransient<ISitesDataAccess, SitesDataAccess>(c => new SitesDataAccess(Configuration.GetConnectionString("default"), loggerFactory));
            services.AddTransient<IPagesDataAccess, PagesDataAccess>(c => new PagesDataAccess(Configuration.GetConnectionString("default"), loggerFactory));
            services.AddTransient<IContentsDataAccess, ContentsDataAccess>(c => new ContentsDataAccess(Configuration.GetConnectionString("default"), loggerFactory));
            services.AddTransient<IMenusDataAccess, MenusDataAccess>(c => new MenusDataAccess(Configuration.GetConnectionString("default"), loggerFactory));
            services.AddTransient<IMetasDataAccess, MetasDataAccess>(c => new MetasDataAccess(Configuration.GetConnectionString("default"), loggerFactory));

            // validators
            services.AddSingleton<IValidator<Content>, ContentValidator>();
            services.AddSingleton<IValidator<Menu>, MenuValidator>();
            services.AddSingleton<IValidator<MenuItem>, MenuItemValidator>();
            services.AddSingleton<IValidator<MetaData>, MetadataValidator>();
            services.AddSingleton<IValidator<Page>, PageValidator>();
            services.AddSingleton<IValidator<Site>, SiteValidator>();


            // services
            services.AddSingleton<IContentService, ContentService>();
            services.AddSingleton<IMenusService, MenusService>();
            services.AddSingleton<IPageService, PagesService>();
            services.AddSingleton<IMetasService, MetasService>();
            services.AddSingleton<ISiteService, SitesService>();

            services.AddHostedService<SeedDataService>();
            services.AddHostedService<CacheLoadService>();

            // Build an intermediate service provider
            var sp = services.BuildServiceProvider();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new GetCmsViewLocator(sp.GetService<ISiteService>()));
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "cmsPage",
                    template: "{lang}/{*pagePath}", // multi language /{lang}/{*pagePath} 
                    defaults: new { controller = "Cms", action = "Index", lang="en", pagePath = "home" },
                    constraints: new { lang = new IsLanguage() });
            });
        }
    }
}
