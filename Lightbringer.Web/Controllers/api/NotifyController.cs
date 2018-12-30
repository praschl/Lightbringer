using System.Threading.Tasks;
using Lightbringer.Web.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Lightbringer.Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        private readonly IHubContext<DaemonHub, IDaemonHub> _context;

        public NotifyController(IHubContext<DaemonHub, IDaemonHub> context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("changed")]
        public async Task<IActionResult> Notify(int id, string daemon, string state)
        {
            await _context.Clients.All.StateChanged(id, daemon, state);

            return Ok();
        }
    }
}