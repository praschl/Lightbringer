using System.Collections.Generic;
using System.Linq;
using Lightbringer.Rest.Contract;

namespace Lightbringer.Web.Store
{
    public class DaemonDtoConverter : IDaemonDtoConverter
    {
        public DaemonVm ToDaemonVm(DaemonDto dto, IReadOnlyCollection<string> subscribedServices)
        {
            return new DaemonVm
            {
                Dto = dto,
                Checked = subscribedServices?.Contains(dto.ServiceName) ?? false
            };
        }
    }
}