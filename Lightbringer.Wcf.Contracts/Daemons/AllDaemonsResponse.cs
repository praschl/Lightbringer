using System.Collections.Generic;

namespace Lightbringer.Wcf.Contracts.Daemons
{
    public class AllDaemonsResponse
    {
        public ICollection<DaemonDto> Daemons { get; set; } = new List<DaemonDto>();
    }
}