using Lightbringer.Rest.Contract;

namespace Lightbringer.Web.Store
{
    public interface IDaemonDtoConverter
    {
        DaemonVm ToDaemonVm(DaemonDto dto, ServiceHost serviceHost);
    }
}