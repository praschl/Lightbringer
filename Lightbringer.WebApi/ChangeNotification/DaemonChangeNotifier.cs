using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Lightbringer.Rest.Contract;

namespace Lightbringer.WebApi.ChangeNotification
{
    public class DaemonChangeNotifier : IDaemonChangeDistributor, IDaemonChangeCallbackRegistry
    {
        private readonly HttpClient _client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });

        private readonly HashSet<string> _registeredUrls = new HashSet<string>();

        public void RegisterCallbackUrl(string url)
        {
            _registeredUrls.Add(url);
        }

        public async Task Distribute(string type, string daemonName, string newState)
        {
            foreach (var url in _registeredUrls)
            {
                var formatted = url
                    .Replace(NotifyParameter.DaemonType, type)
                    .Replace(NotifyParameter.DaemonName, daemonName)
                    .Replace(NotifyParameter.State, newState);
                var result = await _client.GetAsync(formatted);

                // TODO: check result... if not successful, remove url from notification / log
            }
        }
    }
}