using System.Collections.Generic;
using Autofac;
using Lightbringer.Config;
using Lightbringer.WebApi;

namespace Lightbringer.Service.IoC
{
    public class ServiceLocator
    {
        public static IContainer Instance { get; private set; }
            
        public void InitializeDefault()
        {
            Instance = InitContainer();
        }

        private IContainer InitContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new CoreRegistrationModule());
            builder.RegisterModule(new LightbringerServiceModule());
            builder.RegisterModule(new WebApiRegistrationModule(() => Instance));

            var container = builder.Build();

            // NOTE: may pre initialize everything the providers may do to improve performance for the first request with the following line
            // container.Resolve<IEnumerable<IDaemonProvider>>();
            
            return container;
        }
    }
}
