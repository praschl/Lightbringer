using System.Collections.Generic;
using Lightbringer.Web.Core.Store;
using Lightbringer.Web.Core.ViewModels;

namespace Lightbringer.Web.Models
{
    public class DaemonHostViewModel
    {
        public DaemonHost DaemonHost { get; set; }

        public IReadOnlyCollection<DaemonVm> Daemons { get; set; }

        public string Error { get; set; }
    }
}