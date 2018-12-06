using Lightbringer.Web.Store;
using Microsoft.AspNetCore.Mvc;

namespace Lightbringer.Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribeController : ControllerBase
    {
        private readonly IStore _store;

        public SubscribeController(IStore store)
        {
            _store = store;
        }

        public object Toggle(string host, string service)
        {
            return true;
        }
    }
}