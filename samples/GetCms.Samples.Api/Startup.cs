using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace GetCms.Samples.Api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //var loggerFactory = new LoggerFactory();
            //services.AddSingleton<ILoggerFactory, LoggerFactory>();

            //// dataaccess
            //services.AddTransient<ISitesDataAccess, SitesDataAccess>(c => new SitesDataAccess(Configuration.GetConnectionString("default"), loggerFactory));
            //services.AddTransient<IPagesDataAccess, PagesDataAccess>(c => new PagesDataAccess(Configuration.GetConnectionString("default"), loggerFactory));
            //services.AddTransient<IContentsDataAccess, ContentsDataAccess>(c => new ContentsDataAccess(Configuration.GetConnectionString("default"), loggerFactory));
            //services.AddTransient<IMenusDataAccess, MenusDataAccess>(c => new MenusDataAccess(Configuration.GetConnectionString("default"), loggerFactory));
            //services.AddTransient<IMetasDataAccess, MetasDataAccess>(c => new MetasDataAccess(Configuration.GetConnectionString("default"), loggerFactory));

            //// validators
            //services.AddSingleton<IValidator<Content>, ContentValidator>();
            //services.AddSingleton<IValidator<Menu>, MenuValidator>();
            //services.AddSingleton<IValidator<MenuItem>, MenuItemValidator>();
            //services.AddSingleton<IValidator<MetaData>, MetadataValidator>();
            //services.AddSingleton<IValidator<Page>, PageValidator>();
            //services.AddSingleton<IValidator<Site>, SiteValidator>();


            //// services
            //services.AddSingleton<IContentService, ContentService>();
            //services.AddSingleton<IMenusService, MenusService>();
            //services.AddSingleton<IPageService, PagesService>();
            //services.AddSingleton<IMetasService, MetasService>();
            //services.AddSingleton<ISiteService, SitesService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Test API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env/*, ILoggerFactory loggerFactory*/)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
                });
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
