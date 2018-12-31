using System.Collections.Generic;
using Lightbringer.Web.Store;
using Lightbringer.Web.Store.Store;
using Lightbringer.Web.Store.ViewModels;

namespace Lightbringer.Web.Models
{
    public class ServiceHostViewModel
    {
        public ServiceHost ServiceHost { get; set; }

        public IReadOnlyCollection<DaemonVm> Daemons { get; set; }

        public string Error { get; set; }
    }
}