using System;
using Autofac;
using Lightbringer.Service.IoC;
using MiP.Core.Services;

namespace Lightbringer.Service
{
    public static class Program
    {
        static void Main(string[] args)
        {
            new ServiceLocator().InitializeDefault();
            ServiceRunner.Run(ServiceLocator.Instance.Resolve<IServiceProvider>(), args);
        }
    }
}
