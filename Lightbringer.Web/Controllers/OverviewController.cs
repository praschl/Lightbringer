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
        // local:   http://localhost:8080/lightbringer/api
        
        private readonly IDaemonApiProvider _daemonApiProvider;
        private readonly Func<IStore> _store;

        public OverviewController(IDaemonApiProvider daemonApiProvider, Func<IStore> store)
        {
            _daemonApiProvider = daemonApiProvider;
            _store = store;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new AddServiceHostViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddServiceHostUrl(AddServiceHostViewModel viewModel)
        {
            try
            {
                var url = viewModel.ServiceHostUrl;

                if (string.IsNullOrEmpty(url))
                    return RedirectToAction(nameof(Index));

                if (!url.Contains(':') && !url.Contains('/'))
                    url = $"http://{url}:8080/lightbringer/api";

                var uri = new Uri(url); // try to parse url, if not valid, just fail.
                url = uri.ToString();

                var serviceHost = _store().Find(url);
                if (serviceHost == null)
                {
                    var isAliveApi = _daemonApiProvider.GetIsAliveApi(url);

                    await isAliveApi.Get();

                    serviceHost = new ServiceHost
                    {
                        Name = url,
                        Url = viewModel.ServiceHostUrl
                    };

                    _store().Upsert(serviceHost);
                }

                return RedirectToAction("ListServices", new {serviceHostId = serviceHost.Id});
            }
            catch (Exception ex)
            {
                viewModel.Error = ex.Message;
                return View(nameof(Index), viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListServices(int serviceHostId, string filter = null)
        {
            var serviceHost = _store().Get(serviceHostId);
            if (serviceHost == null)
                return NotFound();
            
            var model = new ListServicesViewModel {ServiceHostId = serviceHostId, Filter = filter };

            var daemons = await GetDaemonDtos(serviceHost.Url, model.Filter);
            model.Daemons = daemons;

            return View(model);
        }
        
        private async Task<DaemonVm[]> GetDaemonDtos(string url, string filter)
        {
            var daemonApi = _daemonApiProvider.GetDaemonApi(url);

            var daemons = (await daemonApi.GetDaemons(filter))
                .Select(Convert);

            return daemons.ToArray();
        }

        private DaemonVm Convert(DaemonDto dto)
        {
            return new DaemonVm {Dto = dto};
        }
    }
}