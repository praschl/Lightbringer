using System.Collections.Generic;
using Lightbringer.Rest.Contract;

namespace Lightbringer.Web.Models
{
    public class OverviewViewModel
    {
        public string ServiceHostUrl { get; set; }
        public string Filter { get; set; }

        public IReadOnlyCollection<DaemonDto> AllDaemons { get; set; }

        public string Message { get; set; }
        public string Error { get; set; }
    }
}