using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Lightbringer.WebApi.ChangeNotification;

namespace Lightbringer.WebApi
{
    // TODO: rework wording of "daemon" and "service" in whole solution
    // "Daemon" is anything displayed to the client. It can be a Win32 Service, Application Pool or a Process
    // "Service" should only be used for Win32 services

    // not implementing the IDaemonApi interface enables us to return IHttpActionResult and provide better error handling.

    [RoutePrefix("api/daemons")]
    public class DaemonsController : ApiController
    {
        private readonly IDaemonProvider _daemonProvider;
        private readonly IDaemonChangeCallbackRegistry _daemonChangeCallbackRegistry;

        public DaemonsController(IDaemonProvider daemonProvider, IDaemonChangeCallbackRegistry daemonChangeCallbackRegistry)
        {
            _daemonProvider = daemonProvider;
            _daemonChangeCallbackRegistry = daemonChangeCallbackRegistry;
        }

        [HttpGet]
        [Route("find")]
        public async Task<IHttpActionResult> FindDaemons(string contains = null)
        {
            var result = await _daemonProvider.FindDaemonsAsync(contains);

            return Json(result.ToArray());
        }

        [HttpGet]
        [Route("details")]
        public async Task<IHttpActionResult> GetDaemons([FromUri] string[] names)
        {
            if (names == null)
                return BadRequest();

            var result = await _daemonProvider.GetDaemonsAsync(names);
            return Json(result.ToArray());
        }

        [HttpGet]
        [Route("notify")]
        public IHttpActionResult Notify(string url)
        {
            _daemonChangeCallbackRegistry.RegisterCallbackUrl(url);

            return Ok();
        }
    }
}