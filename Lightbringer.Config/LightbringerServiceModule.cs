using Autofac;
using Lightbringer.WebApi;
using Lightbringer.WebApi.ChangeNotification;
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
            
            builder.RegisterType<Win32ServiceManager>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<DaemonChangeNotifier>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
