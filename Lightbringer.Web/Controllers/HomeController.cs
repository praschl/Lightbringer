﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Lightbringer.Rest.Contract;
using Lightbringer.Web.Models;
using Lightbringer.Web.Store;
using Microsoft.AspNetCore.Mvc;

namespace Lightbringer.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDaemonDtoConverter _daemonDtoConverter;
        private readonly IRestApiProvider _restApiProvider;
        private readonly Func<IStore> _store;

        public HomeController(Func<IStore> store, IRestApiProvider restApiProvider, IDaemonDtoConverter daemonDtoConverter)
        {
            _store = store;
            _restApiProvider = restApiProvider;
            _daemonDtoConverter = daemonDtoConverter;
        }

        public async Task<IActionResult> Index()
        {
            var hosts = _store().FindAllHosts();

            var daemonHosts = (await Task.WhenAll(hosts.Select(GetDaemons)).ContinueWith(r => r.Result))
                .ToArray();

            return View(daemonHosts);
        }

        private async Task<ServiceHostViewModel> GetDaemons(ServiceHost serviceHost)
        {
            try
            {
                var api = _restApiProvider.Get<IDaemonApi>(serviceHost.Url);

                var daemons = await api.GetDaemons(serviceHost.SubscribedServices.ToArray());

                // we pass this url to the Lightbringer.Service.
                // when one of its daemons changes, it will notify us by calling this url.

                // TODO: also add the service name and the new state to the url parameters
                string url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/api/notify/changed?id={serviceHost.Id}";
                await api.Notify(url);

                var daemonVms = daemons
                    .Select(d => _daemonDtoConverter.ToDaemonVm(d, serviceHost.SubscribedServices))
                    .ToArray();

                return new ServiceHostViewModel {ServiceHost = serviceHost, Daemons = daemonVms};
            }
            catch (Exception ex)
            {
                return new ServiceHostViewModel {ServiceHost = serviceHost, Error = ex.Message};
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}