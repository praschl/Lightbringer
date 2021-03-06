﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Lightbringer.WebApi.ChangeNotification;

namespace Lightbringer.WebApi
{
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

        [HttpGet]
        [Route("start")]
        public async Task<IHttpActionResult> Start(string type, string name)
        {
            // ignoring type for now since we currently only have Win32 services

            await _daemonProvider.StartAsync(name);

            return Ok();
        }

        [HttpGet]
        [Route("stop")]
        public async Task<IHttpActionResult> Stop(string type, string name)
        {
            // ignoring type for now since we currently only have Win32 services

            await _daemonProvider.StopAsync(name);

            return Ok();
        }
    }
}