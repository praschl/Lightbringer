using Lightbringer.Rest.Contract;
using Lightbringer.Web.Core.Store;
using Lightbringer.Web.Core.ViewModels;

namespace Lightbringer.Web.Core
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