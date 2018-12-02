using System;
using System.Collections.Generic;
using Lightbringer.Wcf.Contracts.Daemons;
using Lightbringer.Web.Store;
using Microsoft.AspNetCore.Mvc;

namespace Lightbringer.Web.Controllers
{
    public class OverviewController : Controller
    {
        private readonly Func<IStore> _store;
        private readonly IDaemonService _daemonService;

        [BindProperty]
        public OverviewViewModel OverviewViewModel { get; set; } = new OverviewViewModel();

        [TempData] public string Message { get; set; }

        public OverviewController(IDaemonService daemonService, Func<IStore> store)
        {
            _daemonService = daemonService;
            _store = store;
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
            if (!ModelState.IsValid)
            {
                Message = "Servicehost not added!";
                return RedirectToAction(nameof(Index));
            }

            _store().AddServiceHost(OverviewViewModel.AddServiceHost);

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