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
        public Task<DaemonDto[]> GetDaemons(string contains = null)
        {
            return _win32ServiceManager.GetDaemonsAsync(contains);
        }
    }
}