using System.Collections.Generic;
using Lightbringer.Web.Store.ViewModels;

namespace Lightbringer.Web.Models
{
    public class ListDaemonsViewModel
    {
        public int HostId { get; set; }

        public string Filter { get; set; }

        public IReadOnlyCollection<DaemonVm> Daemons { get; set; } = new DaemonVm[0];

        public string ViewType { get; set; }

        public string Error { get; set; }
    }
}