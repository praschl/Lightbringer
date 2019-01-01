using System.Collections.Generic;
using Lightbringer.Web.Store;
using Lightbringer.Web.Store.Store;
using Lightbringer.Web.Store.ViewModels;

namespace Lightbringer.Web.Models
{
    public class DaemonHostViewModel
    {
        public DaemonHost DaemonHost { get; set; }

        public IReadOnlyCollection<DaemonVm> Daemons { get; set; }

        public string Error { get; set; }
    }
}