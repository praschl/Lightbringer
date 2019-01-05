using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Logging;
using Lightbringer.Rest.Contract;

namespace Lightbringer.WebApi.ChangeNotification
{
    public class DaemonChangeNotifier : IDaemonChangeDistributor, IDaemonChangeCallbackRegistry
    {
        private static readonly ILog _log = LogManager.GetLogger<DaemonChangeNotifier>();

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

                _log.DebugFormat("notifying url {0}", formatted);
                var result = await _client.GetAsync(formatted);
                _log.DebugFormat("notify done");

                if (!result.IsSuccessStatusCode)
                {
                    _log.WarnFormat("Could not notify {0}, reason: {1}", formatted, result.ReasonPhrase);
                    // TODO: maybe remove this url, when it returns too many errors?
                }
            }
        }
    }
}