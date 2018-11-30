using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Lightbringer.Wcf.Contracts.Daemons
{
    [DataContract(Namespace = Namespaces.Daemons)]
    public class AllDaemonsResponse
    {
        [DataMember(Order = 1)]
        public IReadOnlyCollection<DaemonDto> Daemons { get; set; } = new List<DaemonDto>();
    }
}