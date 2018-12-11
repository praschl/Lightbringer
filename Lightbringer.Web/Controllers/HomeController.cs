using System;
using System.Collections.Generic;
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
        private readonly IRestApiProvider _restApiProvider;
        private readonly IDaemonDtoConverter _daemonDtoConverter;
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

            var daemons = (await Task.WhenAll(hosts.Select(GetDaemons)).ContinueWith(r => r.Result))
                .SelectMany(m => m)
                .ToArray();

            return View(daemons);
        }

        private async Task<IReadOnlyCollection<DaemonVm>> GetDaemons(ServiceHost serviceHost)
        {
            var api = _restApiProvider.Get<IDaemonApi>(serviceHost.Url);

            // TODO: replace api.GetDaemons() with a HttpPost to api.SubscribeToDaemons()
            // - serviceHost.SubscribedServices
            // - RestUrl where ChangeNotifications should be posted.
            // this call should then return the services, and also notify (via rest) when a service state changed
            var daemons = await api.GetDaemons("");

            var daemonVms = daemons
                .Join(serviceHost.SubscribedServices, d => d.ServiceName, sh => sh, (d, sh) => d)
                .Select(d => _daemonDtoConverter.ToDaemonVm(d, serviceHost))
                .ToArray();

            return daemonVms;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}