using Lightbringer.Rest.Contract;
using Lightbringer.Web.Store.Store;
using Lightbringer.Web.Store.ViewModels;

namespace Lightbringer.Web.Store
{
    public interface IDaemonDtoConverter
    {
        DaemonVm ToDaemonVm(DaemonDto dto, DaemonHost daemonHost);
    }
}