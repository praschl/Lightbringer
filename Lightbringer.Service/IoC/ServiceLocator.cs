using Autofac;
using Lightbringer.Config;

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

            builder.RegisterModule(new LightbringerServiceModule());

            var container = builder.Build();

            return container;
        }
    }
}
