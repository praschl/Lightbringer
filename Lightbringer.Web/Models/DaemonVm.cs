using Lightbringer.Rest.Contract;

namespace Lightbringer.Web.Models
{
    public class DaemonVm // Vm ... part of a view model
    {
        public DaemonDto Dto { get; set; }

        public bool Checked { get; set; }
    }
}