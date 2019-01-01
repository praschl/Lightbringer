using Lightbringer.Web.Core.Store;
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
        public IActionResult Toggle(ToggleDaemonParameter toggleDaemon)
        {
            var host = _store.Get(toggleDaemon.HostId);
            if (host == null)
                return NotFound();

            var index = host.SubscribedDaemons.IndexOf(toggleDaemon.Name);
            if (index >= 0)
                host.SubscribedDaemons.RemoveAt(index);
            else
                host.SubscribedDaemons.Add(toggleDaemon.Name);

            _store.Upsert(host);

            var isNowSubscribed = index < 0;

            return Ok(isNowSubscribed);
        }
    }
}