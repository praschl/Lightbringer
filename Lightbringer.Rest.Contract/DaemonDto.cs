namespace Lightbringer.Rest.Contract
{
    public class DaemonDto
    {
        public string DaemonName { get; set; }

        public string DisplayName { get; set; }

        public string DaemonType { get; set; }

        public string Description { get; set; }

        public string State { get; set; } = DaemonStates.Unknown;
    }
}