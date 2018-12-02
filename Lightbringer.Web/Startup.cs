using System.ServiceModel;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.ServiceModel.Description;
using Autofac.Integration.Wcf;
using Lightbringer.Wcf.Contracts.Daemons;
using Lightbringer.Web.Store;

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
        }

        // called by autofac initialization
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //RegisterWcfServiceClient<IDaemonService>(builder, "daemons");

            builder.RegisterType<LiteDbStore>().AsImplementedInterfaces();
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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        //private void RegisterWcfServiceClient<TContract>(ContainerBuilder builder, string relativeUrl, params IEndpointBehavior[] endpointBehaviors)
        //{
        //    string serviceBaseUrl = Configuration.GetSection("WcfServices.RootUrl").Value;

        //    // auto-implementierung von WCF Services konfigurieren
        //    builder
        //        .Register(c =>
        //        {
        //            var channelFactory = new ChannelFactory<TContract>(
        //                new BasicHttpBinding {MaxReceivedMessageSize = 250_000},
        //                new EndpointAddress(serviceBaseUrl + relativeUrl));

        //            return channelFactory;
        //        })
        //        .SingleInstance();

        //    builder
        //        .Register(componentContext =>
        //        {
        //            var contract = CreateChannel<TContract>(endpointBehaviors, componentContext);

        //            return contract;
        //        })
        //        .As<TContract>()
        //        .UseWcfSafeRelease();
        //}

        //private TContract CreateChannel<TContract>(IEndpointBehavior[] endpointBehaviors, IComponentContext componentContext)
        //{
        //    var channelFactory = componentContext.Resolve<ChannelFactory<TContract>>();

        //    foreach (var behavior in endpointBehaviors)
        //    {
        //        channelFactory.Endpoint.EndpointBehaviors.Add(behavior);
        //    }

        //    return channelFactory.CreateChannel();
        //}
    }
}
