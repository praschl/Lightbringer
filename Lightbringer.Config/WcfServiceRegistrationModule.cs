using System;
using System.Configuration;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Description;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using Autofac.Integration.Wcf;
using Lightbringer.Service.Daemons;
using Lightbringer.Wcf.Contracts.Daemons;
using Lightbringer.Wcf.ServiceModules;
using MiP.Core.Logging;
using MiP.Core.Services;

namespace Lightbringer.Config
{
    public class WcfServiceRegistrationModule : Module
    {
        private static readonly string _serviceBaseUrl = ConfigurationManager.AppSettings["WcfServices.RootUrl"];

        private readonly Func<ILifetimeScope> _dependencyResolverProvider;

        public WcfServiceRegistrationModule(Func<ILifetimeScope> dependencyResolverProvider)
        {
            // Func<ILifetimeScope> deswegen, weil bei der Initialisierung des Modules, der Autofac Container
            // noch nicht fertig ist, und ServiceLocator.Instance zu diesem Zeitpunkt noch NULL ist.
            // bei einem Resolve() wird in (1) SetDependencyResolver einmalig ausgeführt bevor die Instanz
            // an den Aufrufer von Resolve() übergeben wird und in (2) wird dann der DependencyResolver gesetzt,
            // welcher (hoffentlich) auf ServiceLocator.Instance zeigt - zum Zeitpunkt des Resolve() ist dieses
            // Property dann schon gesetzt.
            _dependencyResolverProvider = dependencyResolverProvider;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // hier alle services registrieren
            RegisterWcfServiceHost<IDaemonService, DaemonService>(builder, "daemonService");
        }

        private void RegisterWcfServiceHost<TContract, TService>(ContainerBuilder builder, string relativeUrl, params IServiceBehavior[] behaviors) where TService : TContract
        {
            builder.RegisterType<TService>()
                .As<TContract>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof (LoggerInterceptor))
                ;

            var address = new Uri(_serviceBaseUrl + relativeUrl);

            builder.Register(c => new WcfServiceModule(CreateServiceHost<TContract, TService>(address, behaviors)))
                .As<IServiceModule>()
                // (1) 
                .OnActivated(args => SetDependencyResolver<TContract>(args.Instance.ServiceHost))
                ;
        }

        private static ServiceHost CreateServiceHost<TContract, TService>(Uri address, IServiceBehavior[] behaviors) where TService : TContract
        {
            // ServiceHost erstellen
            // TODO aufräumen: Erstellung des ServiceHost als delegate für autofac registrieren (ggf named auf "wcf-"+relativeurl)
            // TODO: wcfservicemodule mit interception versehen (irgendwas geht da nicht :-)
            // NOTE: der ServiceHost selbst wird bereits über die NLogBehavior bzw NLogErrorHandler mit getraced.
            var serviceHost = new ServiceHost(typeof (TService), address);

            serviceHost.Description.Behaviors.Insert(0, new NLogBehavior(typeof (TService)));

            // add additional behaviours
            foreach (var serviceBehavior in behaviors)
            {
                serviceHost.Description.Behaviors.Add(serviceBehavior);
            }

            SetDebugBehaviour(serviceHost);

            serviceHost.AddServiceEndpoint(typeof (TContract), new BasicHttpBinding(), string.Empty);

            serviceHost.Description.Behaviors.Add(new ServiceMetadataBehavior
            {
                HttpGetEnabled = true,
                HttpGetUrl = address
            });
            return serviceHost;
        }

        private void SetDependencyResolver<TContract>(ServiceHost serviceHost)
        {
            // (2) ILifetimeScope auf den globalen Container setzen.
            serviceHost
                .AddDependencyInjectionBehavior(typeof (TContract), _dependencyResolverProvider());
        }

        [Conditional("DEBUG")]
        private static void SetDebugBehaviour(ServiceHost serviceHost)
        {
            var debug = serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>();

            // if not found - add behavior with setting turned on 
            if (debug == null)
            {
                serviceHost.Description.Behaviors.Add(
                    new ServiceDebugBehavior
                    {
                        IncludeExceptionDetailInFaults = true
                    });
            }
            else
            {
                // make sure setting is turned ON
                if (!debug.IncludeExceptionDetailInFaults)
                {
                    debug.IncludeExceptionDetailInFaults = true;
                }
            }
        }
    }
}