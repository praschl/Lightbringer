using System.Threading.Tasks;
using System.Web.Http;
using Lightbringer.Rest.Contract;

namespace Lightbringer.WebApi
{
    public class IsAliveController : ApiController, IIsAliveApi
    {
        [HttpGet]
        public Task<bool> Get()
        {
            return Task.FromResult(true);
        }
    }
}