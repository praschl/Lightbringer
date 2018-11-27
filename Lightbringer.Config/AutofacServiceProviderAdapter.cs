using System;
using Autofac;

namespace Lightbringer.Config
{
    public class AutofacServiceProviderAdapter : IServiceProvider
    {
        private readonly IComponentContext _context;

        public AutofacServiceProviderAdapter(IComponentContext context)
        {
            _context = context;
        }

        public object GetService(Type serviceType)
        {
            return _context.Resolve(serviceType);
        }
    }
}
