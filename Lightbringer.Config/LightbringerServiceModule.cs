using Autofac;
using Lightbringer.WebApi;
using MiP.Core.Services;

namespace Lightbringer.Config
{
    public class LightbringerServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new AutofacServiceProviderAdapter(c.Resolve<IComponentContext>())).AsImplementedInterfaces();

            builder.RegisterType<CoreService>()
                .OnActivating(c => c.Instance.ServiceName = "LightbringerServiceHost");

            Win32ServiceManager.InitializeAsync(); // we will let this work in the background.
        }
    }
}
