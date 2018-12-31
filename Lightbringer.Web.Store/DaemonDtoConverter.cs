using Lightbringer.Rest.Contract;
using Lightbringer.Web.Store.Store;
using Lightbringer.Web.Store.ViewModels;

namespace Lightbringer.Web.Store
{
    public class DaemonDtoConverter : IDaemonDtoConverter
    {
        public DaemonVm ToDaemonVm(DaemonDto dto, ServiceHost serviceHost)
        {
            var isChecked = serviceHost.SubscribedServices?.Contains(dto.ServiceName) ?? false;

            return new DaemonVm(
                serviceHost.Id, serviceHost.Name,
                dto.ServiceType, dto.ServiceName, dto.DisplayName, dto.Description, dto.State,
                isChecked
            );
        }
    }
}