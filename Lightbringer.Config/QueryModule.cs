using Autofac;
using Lightbringer.Wcf.Daemons;

namespace Lightbringer.Config
{
    public class QueryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GetAllDaemonsQuery>().AsImplementedInterfaces();
        }
    }
}