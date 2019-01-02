using Autofac.Builder;
using Autofac.Extras.DynamicProxy2;
using MiP.Core.Logging;

namespace Lightbringer.Config
{
    public static class InterceptionExtensions
    {
        public static IRegistrationBuilder<T1, T2, T3> InterceptInterfaces<T1, T2, T3>(this IRegistrationBuilder<T1, T2, T3> builder)
        {
            return 
                builder
                    .EnableInterfaceInterceptors()
                    .InterceptedBy(typeof(LoggerInterceptor));
        }
    }
}