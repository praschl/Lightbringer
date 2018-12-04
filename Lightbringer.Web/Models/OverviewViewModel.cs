using System.Collections.Generic;
using Lightbringer.Web.Controllers;

namespace Lightbringer.Web.Models
{
    public class OverviewViewModel
    {
        public string ServiceHostUrl { get; set; }
        public string Filter { get; set; }

        public IReadOnlyList<DaemonVm> AllDaemons { get; set; }

        public string Message { get; set; }
        public string Error { get; set; }
    }
}