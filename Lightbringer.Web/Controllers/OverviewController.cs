using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lightbringer.Rest.Contract;
using Lightbringer.Web.Configuration;
using Lightbringer.Web.Models;
using Lightbringer.Web.Store;
using Lightbringer.Web.Store.Store;
using Lightbringer.Web.Store.ViewModels;
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

            var addDaemonHostViewModel = new AddDaemonHostViewModel
            {
                Hosts = allHosts
            };

            return View(addDaemonHostViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDaemonHostUrl(AddDaemonHostViewModel viewModel)
        {
            try
            {
                var url = viewModel.DaemonHostUrl;

                if (string.IsNullOrEmpty(url))
                    return RedirectToAction(nameof(Index));

                if (!url.Contains(':') && !url.Contains('/')) url = _configuration.WebApiUrlTemplate.Replace("{hostName}", url);

                var uri = new Uri(url); // try to parse url, if not valid, just fail.
                url = uri.ToString();

                var daemonHost = _store().Find(url);
                if (daemonHost == null)
                {
                    var isAliveApi = _restApiProvider.Get<IIsAliveApi>(url);

                    await isAliveApi.Get();

                    daemonHost = new DaemonHost
                    {
                        Name = viewModel.DaemonHostUrl,
                        Url = url
                    };

                    _store().Upsert(daemonHost);
                }

                return RedirectToAction(nameof(ListDaemons), new {hostId = daemonHost.Id});
            }
            catch (Exception ex)
            {
                viewModel.Error = ex.Message;
                return View(nameof(Index), viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListDaemons(int hostId, string filter = null, string viewType = null)
        {
            try
            {
                var daemonHost = _store().Get(hostId);

                if (daemonHost == null)
                    return NotFound();

                var model = new ListDaemonsViewModel { HostId = hostId, Filter = filter, ViewType = viewType ?? "cards" };

                var daemons = await GetDaemonDtos(daemonHost, model.Filter);
                model.Daemons = daemons
                    .OrderBy(d => d.DisplayName)
                    .ThenBy(d => d.DaemonName)
                    .ToArray();

                return View(model);
            }
            catch (Exception ex)
            {
                return View(new ListDaemonsViewModel {Error = ex.Message});
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            _store().Delete(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<DaemonVm>> GetDaemonDtos(DaemonHost daemonHost, string filter)
        {
            var url = daemonHost.Url;

            var daemonApi = _restApiProvider.Get<IDaemonApi>(url);

            var daemons = (await daemonApi.FindDaemons(filter))
                .Select(d => _daemonDtoConverter.ToDaemonVm(d, daemonHost));

            return daemons;
        }

    }
}