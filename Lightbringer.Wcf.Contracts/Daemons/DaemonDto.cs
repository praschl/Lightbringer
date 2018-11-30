using System.Runtime.Serialization;

namespace Lightbringer.Wcf.Contracts.Daemons
{
    [DataContract(Namespace = Namespaces.Daemons)]
    public class DaemonDto
    {
        [DataMember(Order = 1)]
        public string ServiceName { get; set; }

        [DataMember(Order = 2)]
        public string DisplayName { get; set; }

        [DataMember(Order = 3)]
        public string Description { get; set; }

        [DataMember(Order = 4)]
        public string State { get; set; } = DaemonStates.Unknown;
    }
}