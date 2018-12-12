using System.Collections.Generic;
using Lightbringer.Web.Store;

namespace Lightbringer.Web.Models
{
    public class ListServicesViewModel
    {
        public int ServiceHostId { get; set; }

        public string Filter { get; set; }

        public IReadOnlyCollection<DaemonVm> Daemons { get; set; } = new DaemonVm[0];

        public string ViewType { get; set; }

        public string Error { get; set; }
    }
}