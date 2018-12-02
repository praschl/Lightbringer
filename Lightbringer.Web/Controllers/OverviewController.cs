using System.Collections.Generic;
using Lightbringer.Wcf.Contracts.Daemons;
using Microsoft.AspNetCore.Mvc;

namespace Lightbringer.Web.Controllers
{
    public class OverviewController : Controller
    {
        private readonly IDaemonService _daemonService;

        [BindProperty]
        public OverviewViewModel OverviewViewModel { get; set; } = new OverviewViewModel();

        [TempData] public string Message { get; set; }

        public OverviewController(IDaemonService daemonService)
        {
            _daemonService = daemonService;
        }

        public IActionResult Index()
        {
            var response = _daemonService.GetAllDaemons(new AllDaemonsRequest());

            OverviewViewModel.AllDaemons = response.Daemons;
            OverviewViewModel.Message = Message;

            return View(OverviewViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddServiceHost()
        {
            Message = "Servicehost added.";

            return RedirectToAction(nameof(Index));
        }
    }

    public class OverviewViewModel
    {
        public IReadOnlyCollection<DaemonDto> AllDaemons { get; set; }

        public string AddServiceHost { get; set; }
  
        public string Message { get; set; }
    }
}