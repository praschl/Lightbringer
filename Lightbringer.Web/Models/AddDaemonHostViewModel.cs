using System.Collections.Generic;
using Lightbringer.Web.Core.Store;

namespace Lightbringer.Web.Models
{
    public class AddDaemonHostViewModel
    {
        public string DaemonHostUrl { get; set; }

        public string Error { get; set; }
        public IReadOnlyCollection<DaemonHost> Hosts { get; set; }
    }
}