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
        private readonly IDaemonApiProvider _daemonApiProvider;
        private readonly Func<IStore> _store;

        [BindProperty]
        public OverviewViewModel OverviewViewModel { get; set; } = new OverviewViewModel();

        [TempData] public string Message { get; set; }
        [TempData] public string Error { get; set; }

        public OverviewController(IDaemonApiProvider daemonApiProvider, Func<IStore> store)
        {
            _daemonApiProvider = daemonApiProvider;
            _store = store;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(OverviewViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListServices()
        {
            if (!string.IsNullOrEmpty(OverviewViewModel.ServiceHostUrl))
            {
                // local:   http://localhost:8080/lightbringer/api

                var daemons = await TryGetDaemonDtos();

                OverviewViewModel.AllDaemons = daemons;
            }

            OverviewViewModel.Message = Message;
            OverviewViewModel.Error = Error;

            return View(nameof(Index), OverviewViewModel);
        }

        private async Task<DaemonDto[]> TryGetDaemonDtos()
        {
            try
            {
                string url = OverviewViewModel.ServiceHostUrl;
                if (!url.Contains(':') && !url.Contains('/'))
                {
                    url = $"http://{url}:8080/lightbringer/api";
                }

                var daemonApi = _daemonApiProvider.Get(url);

                var daemons = await daemonApi.GetAllDaemonsAsync();

                return daemons;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return new DaemonDto[0];
            }
        }
    }

    public class OverviewViewModel
    {
        public string ServiceHostUrl { get; set; }
        public string Filter { get; set; }

        public IReadOnlyCollection<DaemonDto> AllDaemons { get; set; }

        public string Message { get; set; }
        public string Error { get; set; }
    }
}