using System;
using System.Linq;
using System.Threading.Tasks;
using Lightbringer.Rest.Contract;
using Lightbringer.Web.Models;
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

            return View(nameof(Index), OverviewViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterServices()
        {
            var checkedDaemons = OverviewViewModel.AllDaemons
                .Where(c => c.Checked)
                .Select(c => c.Dto.ServiceName)
                .ToArray();

            // TODO: register these services

            return View(nameof(Index), OverviewViewModel);
        }

        private async Task<DaemonVm[]> TryGetDaemonDtos()
        {
            try
            {
                string url = OverviewViewModel.ServiceHostUrl;
                if (!url.Contains(':') && !url.Contains('/'))
                {
                    url = $"http://{url}:8080/lightbringer/api";
                }

                var daemonApi = _daemonApiProvider.Get(url);

                var daemons = (await daemonApi.GetDaemonsAsync(OverviewViewModel.Filter))
                    .Select(Convert);
                    
                return daemons.ToArray();
            }
            catch (Exception ex)
            {
                OverviewViewModel.Error = ex.Message;
                return new DaemonVm[0];
            }
        }

        private DaemonVm Convert(DaemonDto dto)
        {
            return new DaemonVm {Dto = dto};
        }
    }

    public class DaemonVm // Vm ... part of a view model
    {
        public DaemonDto Dto { get; set; }

        public bool Checked { get; set; }
    }
}