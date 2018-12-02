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
            //builder.RegisterModule(new WcfServiceRegistrationModule(() => Instance));
            builder.RegisterModule(new WebApiRegistrationModule(() => Instance));
            builder.RegisterModule(new QueryModule());

            var container = builder.Build();

            return container;
        }
    }
}
