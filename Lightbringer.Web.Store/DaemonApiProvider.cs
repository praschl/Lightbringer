using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Lightbringer.Rest.Contract;
using RestEase;

namespace Lightbringer.Web.Store
{
    public class DaemonApiProvider : IDaemonApiProvider
    {
        private readonly ConcurrentDictionary<string, IDaemonApi> _daemonApis = new ConcurrentDictionary<string, IDaemonApi>(StringComparer.OrdinalIgnoreCase);

        public IDaemonApi Get(string url)
        {
            return _daemonApis.GetOrAdd(url, u => RestClient.For<IDaemonApi>(url));
        }

        public IReadOnlyCollection<IDaemonApi> GetAll()
        {
            return _daemonApis.Values.ToArray();
        }
    }
}