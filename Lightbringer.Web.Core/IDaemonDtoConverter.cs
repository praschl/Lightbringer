using Lightbringer.Rest.Contract;
using Lightbringer.Web.Core.Store;
using Lightbringer.Web.Core.ViewModels;

namespace Lightbringer.Web.Core
{
    public interface IDaemonDtoConverter
    {
        DaemonVm ToDaemonVm(DaemonDto dto, DaemonHost daemonHost);
    }
}