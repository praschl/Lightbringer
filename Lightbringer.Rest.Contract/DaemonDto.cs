namespace Lightbringer.Rest.Contract
{
    public class DaemonDto
    {
        public string ServiceName { get; set; }

        public string DisplayName { get; set; }

        public string ServiceType { get; set; }

        public string Description { get; set; }

        public string State { get; set; } = DaemonStates.Unknown;
    }
}