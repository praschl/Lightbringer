using System.Threading.Tasks;
using Lightbringer.Rest.Contract;
using Lightbringer.Web.Core;
using Lightbringer.Web.Core.Store;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Lightbringer.Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageDaemonController : ControllerBase
    {
        private readonly ILogger<ManageDaemonController> _log;
        private readonly IRestApiProvider _restApiProvider;
        private readonly IStore _store;

        public ManageDaemonController(IRestApiProvider restApiProvider, IStore store, ILogger<ManageDaemonController> log)
        {
            _restApiProvider = restApiProvider;
            _store = store;
            _log = log;
        }

        [HttpGet]
        [Route("start")]
        public async Task<IActionResult> Start(int id, string type, string daemon)
        {
            var hosts = _store.FindAllHosts();

            // TODO: use Parallel.Foreach here?
            foreach (var daemonHost in hosts)
            {
                // TODO: OK as long as we only have Win32 services
                if (!daemonHost.SubscribedDaemons.Contains(daemon))
                    continue;

                var api = _restApiProvider.Get<IDaemonApi>(daemonHost.Url);


                _log.LogInformation("Sending Start to {0}-{1}...", type, daemon);
                await api.Start(type, daemon)
                    .ContinueWith(t => _log.LogWarning(t.Exception, "Sending start to {0}-{1} failed.", type, daemon),
                        TaskContinuationOptions.OnlyOnFaulted);
            }

            return Ok();
        }

        [HttpGet]
        [Route("stop")]
        public async Task<IActionResult> Stop(int id, string type, string daemon)
        {
            var hosts = _store.FindAllHosts();

            // TODO: use Parallel.Foreach here?
            foreach (var daemonHost in hosts)
            {
                // TODO: OK as long as we only have Win32 services
                if (!daemonHost.SubscribedDaemons.Contains(daemon))
                    continue;

                var api = _restApiProvider.Get<IDaemonApi>(daemonHost.Url);

                _log.LogInformation("Sending Stop to {0}-{1}...", type, daemon);
                await api.Stop(type, daemon)
                    .ContinueWith(t => _log.LogWarning(t.Exception, "Sending stop to {0}-{1} failed.", type, daemon),
                        TaskContinuationOptions.OnlyOnFaulted);
            }

            return Ok();
        }
    }
}