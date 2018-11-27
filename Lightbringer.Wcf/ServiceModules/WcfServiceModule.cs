using MiP.Core.Services;
using System;
using System.ServiceModel;

namespace Lightbringer.Wcf.ServiceModules
{
    public class WcfServiceModule : IServiceModule, IDisposable
    {
        public WcfServiceModule(ServiceHost serviceHost)
        {
            ServiceHost = serviceHost;
        }

        public ServiceHost ServiceHost { get; }

        public void Open()
        {
            ServiceHost.Open();
        }

        public void Close()
        {
            try
            {
                ServiceHost.Close();
            }
            catch (Exception)
            {
                ServiceHost.Abort();
            }
        }

        private bool _disposed;

        public void Dispose()
        {
            if (_disposed)
                return;

            Close();

            _disposed = true;
        }
    }
}