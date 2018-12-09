using System.Threading.Tasks;
using System.Web.Http;
using Lightbringer.Rest.Contract;

namespace Lightbringer.WebApi
{
    public class DaemonsController : ApiController, IDaemonApi
    {
        [HttpGet]
        public Task<DaemonDto[]> GetDaemons(string contains = null)
        {
            return Win32ServiceManager.GetDaemons(contains);
        }
    }
}