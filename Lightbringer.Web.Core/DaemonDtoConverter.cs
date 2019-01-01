using Lightbringer.Rest.Contract;
using Lightbringer.Web.Store.Store;
using Lightbringer.Web.Store.ViewModels;

namespace Lightbringer.Web.Store
{
    public class DaemonDtoConverter : IDaemonDtoConverter
    {
        public DaemonVm ToDaemonVm(DaemonDto dto, DaemonHost daemonHost)
        {
            var isChecked = daemonHost.SubscribedDaemons?.Contains(dto.DaemonName) ?? false;

            return new DaemonVm(
                daemonHost.Id, daemonHost.Name,
                dto.DaemonType, dto.DaemonName, dto.DisplayName, dto.Description, dto.State,
                isChecked
            );
        }
    }
}