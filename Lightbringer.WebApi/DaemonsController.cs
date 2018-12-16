using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Lightbringer.Rest.Contract;

namespace Lightbringer.WebApi
{
    public class DaemonsController : ApiController, IDaemonApi
    {
        private readonly Win32ServiceManager _win32ServiceManager;

        public DaemonsController(Win32ServiceManager win32ServiceManager)
        {
            _win32ServiceManager = win32ServiceManager;
        }

        [HttpGet]
        public async Task<DaemonDto[]> FindDaemons(string contains = null)
        {
            var result = await _win32ServiceManager.FindDaemonsAsync(contains);
            return result.ToArray();
        }

        [HttpPost]
        public async Task<DaemonDto[]> GetDaemons([FromBody] string[] daemonNames)
        {
            var result = await _win32ServiceManager.GetDaemonsAsync(daemonNames);
            return result.ToArray();
        }

        // TODO: rework wording of "daemon" and "service" in whole solution
    }
}