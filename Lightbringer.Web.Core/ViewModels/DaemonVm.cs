namespace Lightbringer.Web.Store.ViewModels
{
    public class DaemonVm // Vm ... part of a view model
    {
        public DaemonVm(int hostId, string hostName, string daemonType, string daemonName, string displayName, string description, string state, bool @checked)
        {
            HostId = hostId;
            HostName = hostName;
            DaemonType = daemonType;
            DaemonName = daemonName;
            DisplayName = displayName;
            Description = description;
            State = state;
            Checked = @checked;
        }

        public int HostId { get; set; }
        public string HostName { get; set; }

        public string DaemonType { get; set; }
        public string DaemonName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        
        public bool Checked { get; set; }
    }
}