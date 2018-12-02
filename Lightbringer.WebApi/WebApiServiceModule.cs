using System;
using System.Web.Http.SelfHost;
using MiP.Core.Services;

namespace Lightbringer.WebApi
{
    public class WebApiServiceModule : IServiceModule, IDisposable
    {
        private readonly HttpSelfHostServer _webapiHost;

        public WebApiServiceModule(HttpSelfHostServer webapiHost)
        {
            _webapiHost = webapiHost;
        }

        public void Open()
        {
            _webapiHost.OpenAsync().Wait();
        }

        public void Close()
        {
            _webapiHost.CloseAsync();
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