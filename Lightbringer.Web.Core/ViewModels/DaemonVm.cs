namespace Lightbringer.Web.Store.ViewModels
{
    public class DaemonVm // Vm ... part of a view model
    {
        public DaemonVm(int hostId, string hostName, string serviceType, string serviceName, string displayName, string description, string state, bool @checked)
        {
            HostId = hostId;
            HostName = hostName;
            ServiceType = serviceType;
            ServiceName = serviceName;
            DisplayName = displayName;
            Description = description;
            State = state;
            Checked = @checked;
        }

        public int HostId { get; set; }
        public string HostName { get; set; }

        public string ServiceType { get; set; }
        public string ServiceName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        
        public bool Checked { get; set; }
    }
}