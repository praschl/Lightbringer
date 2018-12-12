using System.Collections.Generic;
using Lightbringer.Web.Store;

namespace Lightbringer.Web.Models
{
    public class ServiceHostViewModel
    {
        public ServiceHost ServiceHost { get; set; }

        public IReadOnlyCollection<DaemonVm> Daemons { get; set; }

        public string Error { get; set; }
    }
}