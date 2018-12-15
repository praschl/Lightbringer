using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Threading.Tasks;
using Lightbringer.Rest.Contract;

namespace Lightbringer.WebApi
{
    public class Win32ServiceManager
    {
        // we make this static, because all those service controllers should be disposed when no longer needed
        // since we subscribe to the changed event (Event Query), we will require them for the whole lifetime of this process.
        // by staying static, we will just have one instance per service, which is sufficient for us.
        private static readonly ServiceController[] _services = ServiceController.GetServices();

        private static readonly List<DaemonDto> _daemonDtos = new List<DaemonDto>();

        private static readonly Dictionary<string, string> _stateStrings = new Dictionary<string, string>
        {
            {"Running", DaemonStates.Running},
            {"Start Pending", DaemonStates.StartPending},
            {"Stopped", DaemonStates.Stopped},
            {"Stop Pending", DaemonStates.StopPending}
        };

        private static readonly Dictionary<ServiceControllerStatus, string> _stateEnums = new Dictionary<ServiceControllerStatus, string>
        {
            {ServiceControllerStatus.Running, DaemonStates.Running},
            {ServiceControllerStatus.StartPending, DaemonStates.StartPending},
            {ServiceControllerStatus.Stopped, DaemonStates.Stopped},
            {ServiceControllerStatus.StopPending, DaemonStates.StopPending}

            // left these here for future support
            //{ ServiceControllerStatus.ContinuePending, DaemonStates.ContinuePending},
            //{ ServiceControllerStatus.Paused, DaemonStates.Paused},
            //{ ServiceControllerStatus.PausePending, DaemonStates.PausePending},
        };

        public static void InitializeDtos()
        {
            if (_daemonDtos.Count > 0)
                return;

            lock (_daemonDtos)
            {
                if (_daemonDtos.Count > 0)
                    return;

                var daemonControllers = _services.Select(serviceController => new
                {
                    // TODO: return DependentServices and ServiceDependsOn, too, these will be required for stopping and starting.
                    DaemonDto = new DaemonDto
                    {
                        ServiceName = serviceController.ServiceName,
                        DisplayName = serviceController.DisplayName,
                        ServiceType = "Win32 Service",
                        Description = null,
                        State = GetState(serviceController.Status)
                    },
                    ServiceController = serviceController
                });

                foreach (var daemonController in daemonControllers)
                {
                    _daemonDtos.Add(daemonController.DaemonDto);
                    LoadDescriptionAsync(daemonController.ServiceController, daemonController.DaemonDto);
                }

                // Source: https://dotnetcodr.com/2014/12/02/getting-notified-by-a-windows-service-status-change-in-c-net/
                var servicesQuery = new EventQuery();
                // The following would work (with different properties) for Processes
                // WHERE targetinstance isa Win32_Process
                servicesQuery.QueryString = "SELECT * FROM __InstanceModificationEvent within 1 WHERE targetinstance isa 'Win32_Service'";
                var servicesWatcher = new ManagementEventWatcher(servicesQuery);
                servicesWatcher.EventArrived += DemoWatcher_EventArrived;
                servicesWatcher.Start();
            }
        }

        private static void DemoWatcher_EventArrived(object sender, EventArrivedEventArgs eventArgs)
        {
            var serviceObject = (ManagementBaseObject) eventArgs.NewEvent.Properties["TargetInstance"].Value;

            // left this here for future investigation of other properties
            //
            //PropertyDataCollection props = serviceObject.Properties;
            //foreach (PropertyData prop in props)
            //{
            //    Console.WriteLine("{0}: {1}", prop.Name, prop.Value);
            //}

            var serviceName = (string) serviceObject.Properties["Name"].Value;
            var state = (string) serviceObject.Properties["State"].Value;

            var service = _daemonDtos.FirstOrDefault(s => s.ServiceName == serviceName);
            if (service != null) service.State = GetState(state);
        }

        private static void LoadDescriptionAsync(ServiceController serviceController, DaemonDto daemonDto)
        {
            Task.Run(() =>
            {
                try
                {
                    var serviceName = serviceController.ServiceName;
                    using (var managementService = new ManagementObject(new ManagementPath($"Win32_Service.Name='{serviceName}'")))
                    {
                        var description = managementService["Description"]?.ToString() ?? string.Empty;
                        daemonDto.Description = description;
                    }
                }
                catch (Exception ex)
                {
                    daemonDto.Description = "ERROR reading service Description: " + ex.Message;
                }
            });
        }

        public Task<DaemonDto[]> GetDaemonsAsync(string contains)
        {
            IEnumerable<DaemonDto> dtos = _daemonDtos;

            if (!string.IsNullOrWhiteSpace(contains))
                dtos = dtos.Where(d =>
                    d.ServiceName.IndexOf(contains, StringComparison.OrdinalIgnoreCase) >= 0
                    || d.DisplayName.IndexOf(contains, StringComparison.OrdinalIgnoreCase) >= 0
                );

            return Task.FromResult(dtos.ToArray());
        }

        private static string GetState(string status)
        {
            if (_stateStrings.ContainsKey(status))
                return _stateStrings[status];

            return DaemonStates.Unknown;
        }

        private static string GetState(ServiceControllerStatus status)
        {
            if (_stateEnums.ContainsKey(status))
                return _stateEnums[status];

            return DaemonStates.Unknown;
        }
    }
}