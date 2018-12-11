using Lightbringer.Rest.Contract;

namespace Lightbringer.Web.Store
{
    public class DaemonDtoConverter : IDaemonDtoConverter
    {
        public DaemonVm ToDaemonVm(DaemonDto dto, ServiceHost serviceHost)
        {
            return new DaemonVm
            {
                Dto = dto,
                Checked = serviceHost.SubscribedServices?.Contains(dto.ServiceName) ?? false,
                ServiceHost = serviceHost
            };
        }
    }
}