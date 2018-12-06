using System;
using System.Collections.Concurrent;
using Lightbringer.Rest.Contract;
using RestEase;

namespace Lightbringer.Web.Store
{
    public class DaemonApiProvider : IDaemonApiProvider
    {
        private readonly ConcurrentDictionary<string, IDaemonApi> _daemonApis = new ConcurrentDictionary<string, IDaemonApi>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, IIsAliveApi> _isAliveApis = new ConcurrentDictionary<string, IIsAliveApi>(StringComparer.OrdinalIgnoreCase);

        public IDaemonApi GetDaemonApi(string url)
        {
            return _daemonApis.GetOrAdd(url, u => RestClient.For<IDaemonApi>(url));
        }

        public IIsAliveApi GetIsAliveApi(string url)
        {
            return _isAliveApis.GetOrAdd(url, u => RestClient.For<IIsAliveApi>(url));
        }
    }
}