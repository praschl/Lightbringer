using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lightbringer.Rest.Contract;
using Lightbringer.Web.Store;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Refit;

namespace Lightbringer.Web.Controllers
{
    public class OverviewController : Controller
    {
        private readonly Func<IStore> _store;
        private readonly IConfiguration _configuration;

        [BindProperty]
        public OverviewViewModel OverviewViewModel { get; set; } = new OverviewViewModel();

        [TempData] public string Message { get; set; }

        public OverviewController(Func<IStore> store, IConfiguration configuration)
        {
            _store = store;
            _configuration = configuration;
        }

        public async Task< IActionResult> Index()
        {
            var daemonsApi = RestService.For<IDaemonApi>("http://localhost:8080/lightbringer/api");

            var daemons = await daemonsApi.GetAllDaemonsAsync();

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