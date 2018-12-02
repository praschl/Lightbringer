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

            return container;
        }
    }
}
