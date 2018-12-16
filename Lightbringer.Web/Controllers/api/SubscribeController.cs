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

        [HttpPost]
        public IActionResult Toggle(ToggleParameter parameter)
        {
            var serviceHost = _store.Get(parameter.ServiceHostId);
            if (serviceHost == null)
                return NotFound();

            var index = serviceHost.SubscribedServices.IndexOf(parameter.ServiceName);
            if (index >= 0)
                serviceHost.SubscribedServices.RemoveAt(index);
            else
                serviceHost.SubscribedServices.Add(parameter.ServiceName);

            _store.Upsert(serviceHost);

            var isNowSubscribed = index < 0;

            return Ok(isNowSubscribed);
        }
    }
}