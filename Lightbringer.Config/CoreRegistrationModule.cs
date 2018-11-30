using Autofac;
using MiP.Core.Logging;

namespace Lightbringer.Config
{
    public class CoreRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoggerInterceptor>().AsSelf();
        }
    }
}