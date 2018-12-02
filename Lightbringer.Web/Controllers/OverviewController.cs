using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lightbringer.Rest.Contract;
using Lightbringer.Web.Store;
using Microsoft.AspNetCore.Mvc;

namespace Lightbringer.Web.Controllers
{
    public class OverviewController : Controller
    {
        private readonly IDaemonApi _daemonApi;
        private readonly Func<IStore> _store;

        [BindProperty]
        public OverviewViewModel OverviewViewModel { get; set; } = new OverviewViewModel();

        [TempData] public string Message { get; set; }

        public OverviewController(IDaemonApi daemonApi, Func<IStore> store)
        {
            _daemonApi = daemonApi;
            _store = store;
        }

        public async Task< IActionResult> Index()
        {
            var daemons = await _daemonApi.GetAllDaemonsAsync();

            OverviewViewModel.AllDaemons = daemons;
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