using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Autofac;
using Autofac.Integration.WebApi;
using MiP.Core.Services;
using Newtonsoft.Json.Converters;
using Swashbuckle.Application;

namespace Lightbringer.WebApi
{
    public class WebApiRegistrationModule : Module
    {
        private static readonly string _webapiUrl = ConfigurationManager.AppSettings["WebApi.RootUrl"];

        private readonly Func<ILifetimeScope> _dependencyResolverProvider;

        public WebApiRegistrationModule(Func<ILifetimeScope> dependencyResolverProvider)
        {
            _dependencyResolverProvider = dependencyResolverProvider;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(typeof(DaemonsController).Assembly)
                .AsSelf()
                //.EnableClassInterceptors()
                //.InterceptedBy(typeof(LoggerInterceptor))
                ;

            var httpSelfHostServer = CreateHttpSelfHostServer();

            builder.RegisterInstance(new WebApiServiceModule(httpSelfHostServer))
                .As<IServiceModule>()
                .OnActivated(args => SetDependencyResolver(httpSelfHostServer));

            Console.WriteLine("WebApi is hosted at " + _webapiUrl);
            Console.WriteLine("Swagger is hosted at " + _webapiUrl + "swagger");
        }

        private void SetDependencyResolver(HttpSelfHostServer httpSelfHostServer)
        {
            httpSelfHostServer.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(_dependencyResolverProvider());
        }

        private static HttpSelfHostServer CreateHttpSelfHostServer()
        {
            var webapiConfig = new HttpSelfHostConfiguration(_webapiUrl);
            webapiConfig.Routes.MapHttpRoute("API Default", "api/{controller}");
            webapiConfig.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());

            webapiConfig.EnableSwagger(c => c.SingleApiVersion("v1", "Library API"))
                .EnableSwaggerUi();

            var webApiSelfHostServer = new HttpSelfHostServer(webapiConfig);

            return webApiSelfHostServer;
        }
    }
}
