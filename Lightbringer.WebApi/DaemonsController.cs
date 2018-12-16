using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Lightbringer.WebApi
{
    // TODO: rework wording of "daemon" and "service" in whole solution
    // "Daemon" is anything displayed to the client. It can be a Win32 Service, Application Pool or a Process
    // "Service" should only be used for Win32 services

    // not implementing the IDaemonApi interface enables us to return IHttpActionResult and provide better error handling.

    [RoutePrefix("api/daemons")]
    public class DaemonsController : ApiController
    {
        private readonly Win32ServiceManager _win32ServiceManager;

        public DaemonsController(Win32ServiceManager win32ServiceManager)
        {
            _win32ServiceManager = win32ServiceManager;
        }

        [HttpGet]
        [Route("find")]
        public async Task<IHttpActionResult> FindDaemons(string contains = null)
        {
            var result = await _win32ServiceManager.FindDaemonsAsync(contains);

            return Json(result.ToArray());
        }

        [HttpGet]
        [Route("details")]
        public async Task<IHttpActionResult> GetDaemons([FromUri] string[] names)
        {
            if (names == null)
                return BadRequest();

            var result = await _win32ServiceManager.GetDaemonsAsync(names);
            return Json(result.ToArray());
        }

        [HttpGet]
        [Route("notify")]
        public async Task<IHttpActionResult> Notify(string url)
        {
            // this is just demo code on how to call notify the webserver
            // TODO: make a singleton class which stores the urls (distinct), when a service changes, that singleton should call the url

            var handler = new HttpClientHandler {UseDefaultCredentials = true};
            using (HttpClient client = new HttpClient(handler))
            {
                var result = await client.GetAsync(url);
            }

            return Ok();
        }
    }
}