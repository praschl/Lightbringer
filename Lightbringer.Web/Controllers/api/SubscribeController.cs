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

        [HttpGet] // TODO: that should be a Post
        public IActionResult Toggle([FromQuery] int serviceHostId, [FromQuery] string service)
        {
            var serviceHost = _store.Get(serviceHostId);
            if (serviceHost == null)
                return NotFound();

            var index = serviceHost.SubscribedServices.IndexOf(service);
            if (index >= 0)
                serviceHost.SubscribedServices.RemoveAt(index);
            else
                serviceHost.SubscribedServices.Add(service);

            _store.Upsert(serviceHost);

            var isNowSubscribed = index < 0;

            return Ok(isNowSubscribed);
        }
    }
}