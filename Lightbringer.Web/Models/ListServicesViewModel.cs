using System.Collections.Generic;

namespace Lightbringer.Web.Models
{
    public class ListServicesViewModel
    {
        public int ServiceHostId { get; set; }

        public string Filter { get; set; }

        public IReadOnlyCollection<DaemonVm> Daemons { get; set; } = new DaemonVm[0];
    }
}