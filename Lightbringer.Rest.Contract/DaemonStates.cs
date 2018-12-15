namespace Lightbringer.Rest.Contract
{
    public static class DaemonStates
    {
        public const string StartPending = "StartPending";
        public const string Running = "Running";
        public const string StopPending = "StopPending";
        public const string Stopped = "Stopped";

        public const string NotFound = "NotFound";

        public const string Unknown = "Unknown";
    }
}