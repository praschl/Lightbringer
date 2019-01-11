using System.IO;
using Autofac;
using Autofac.Core;
using Lightbringer.Web.Configuration;
using Lightbringer.Web.Core;
using Lightbringer.Web.Core.Store;
using Lightbringer.Web.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lightbringer.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                ;

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSignalR();
        }

        // called by autofac initialization
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<LoggerInterceptor>().AsSelf();

            var webApiUrlTemplate = Configuration["WebApi.Url.Template"];
            builder.Register(c => new LightbringerConfiguration {WebApiUrlTemplate = webApiUrlTemplate}).SingleInstance();

            // TODO: need to find a directory that works with default application pool
            var liteDbConfig = Configuration.GetSection("liteDb").Get<LiteDbStoreConfiguration>()
                               ??
                               new LiteDbStoreConfiguration();
            builder.Register(c => liteDbConfig)
                .OnActivating(ActivateConfig)
                .SingleInstance();

            builder.RegisterType<RestApiProvider>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<LiteDbStore>()
                .InterceptInterfaces()
                .AsImplementedInterfaces();

            builder.RegisterType<DaemonDtoConverter>()
                .InterceptInterfaces()
                .AsImplementedInterfaces();
        }

        private void ActivateConfig(IActivatingEventArgs<LiteDbStoreConfiguration> e)
        {
            var hostingEnvironment = e.Context.Resolve<IHostingEnvironment>();

            var dbDirectory = Path.Combine(hostingEnvironment.ContentRootPath, "database");
            if (!Directory.Exists(dbDirectory))
                Directory.CreateDirectory(dbDirectory);

            e.Instance.DbPath = Path.Combine(dbDirectory, "lite.db");
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSignalR(routes =>
            {
                routes.MapHub<DaemonHub>("/daemonHub"); 
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
