using Microsoft.AspNetCore.Mvc;

namespace Lightbringer.Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        [HttpGet]
        [Route("changed")]
        public IActionResult Notify(int id, string daemon, string state)
        {
            return Ok();
        }
    }
}