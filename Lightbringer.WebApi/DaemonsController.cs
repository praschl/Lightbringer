using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Lightbringer.WebApi
{
    // TODO: rework wording of "daemon" and "service" in whole solution

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

    }
}