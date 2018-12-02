using Lightbringer.Wcf.Contracts.Daemons;
using Microsoft.AspNetCore.Mvc;

namespace Lightbringer.Web.Controllers
{
    public class DaemonsController : Controller
    {
        private readonly IDaemonService _daemonService;

        public DaemonsController(IDaemonService daemonService)
        {
            _daemonService = daemonService;
        }

        public IActionResult Index()
        {
            var response = _daemonService.GetAllDaemons(new AllDaemonsRequest());

            return View(response.Daemons);
        }
    }
}