using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lightbringer.Rest.Contract;
using Lightbringer.Web.Configuration;
using Lightbringer.Web.Models;
using Lightbringer.Web.Store;
using Microsoft.AspNetCore.Mvc;

namespace Lightbringer.Web.Controllers
{
    public class OverviewController : Controller
    {
        // local:   http://localhost:8080/lightbringer/api

        private readonly LightbringerConfiguration _configuration;
        private readonly IRestApiProvider _restApiProvider;
        private readonly Func<IStore> _store;
        private readonly IDaemonDtoConverter _daemonDtoConverter;

        public OverviewController(IRestApiProvider restApiProvider, 
            Func<IStore> store,
            IDaemonDtoConverter daemonDtoConverter,
            LightbringerConfiguration configuration)
        {
            _restApiProvider = restApiProvider;
            _store = store;
            _daemonDtoConverter = daemonDtoConverter;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var allHosts = _store().FindAllHosts();

            var addServiceHostViewModel = new AddServiceHostViewModel
            {
                ServiceHosts = allHosts
            };

            return View(addServiceHostViewModel);
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

                if (!url.Contains(':') && !url.Contains('/')) url = _configuration.WebApiUrlTemplate.Replace("{hostName}", url);

                var uri = new Uri(url); // try to parse url, if not valid, just fail.
                url = uri.ToString();

                var serviceHost = _store().Find(url);
                if (serviceHost == null)
                {
                    var isAliveApi = _restApiProvider.Get<IIsAliveApi>(url);

                    await isAliveApi.Get();

                    serviceHost = new ServiceHost
                    {
                        Name = viewModel.ServiceHostUrl,
                        Url = url
                    };

                    _store().Upsert(serviceHost);
                }

                return RedirectToAction(nameof(ListServices), new {serviceHostId = serviceHost.Id});
            }
            catch (Exception ex)
            {
                viewModel.Error = ex.Message;
                return View(nameof(Index), viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListServices(int serviceHostId, string filter = null, string viewType = null)
        {
            var serviceHost = _store().Get(serviceHostId);
            if (serviceHost == null)
                return NotFound();

            var model = new ListServicesViewModel {ServiceHostId = serviceHostId, Filter = filter, ViewType = viewType ?? "cards"};

            var daemons = await GetDaemonDtos(serviceHost, model.Filter);
            model.Daemons = daemons
                .OrderBy(d => d.Dto.DisplayName)
                .ThenBy(d => d.Dto.ServiceName)
                .ToArray();

            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            _store().Delete(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<DaemonVm>> GetDaemonDtos(ServiceHost serviceHost, string filter)
        {
            var url = serviceHost.Url;

            var daemonApi = _restApiProvider.Get<IDaemonApi>(url);

            var daemons = (await daemonApi.GetDaemons(filter))
                .Select(d => _daemonDtoConverter.ToDaemonVm(d, serviceHost));

            return daemons;
        }

    }
}