using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Threading.Tasks;
using Common.Logging;
using Lightbringer.Rest.Contract;
using Lightbringer.WebApi.ChangeNotification;

namespace Lightbringer.WebApi
{
    public class Win32ServiceManager : IDaemonProvider
    {
        private const string Win32ServiceType = "Win32Service";

        private static readonly ILog _log = LogManager.GetLogger<Win32ServiceManager>();

        private readonly IDaemonChangeDistributor _daemonChangeDistributor;
        private readonly List<DaemonDto> _daemonDtos = new List<DaemonDto>();

        // This class will be registered as a singleton instead of making it static. This makes for easier testing and better separation of concerns.
        // <strike>we make this static</strike>, because all those service controllers should be disposed when no longer needed
        // since we subscribe to the changed event (Event Query), we will require them for the whole lifetime of this process.
        // by staying static, we will just have one instance per service, which is sufficient for us.
        private readonly IDictionary<string, ServiceController> _services = ServiceController.GetServices().ToDictionary(s => s.ServiceName);

        private readonly Dictionary<ServiceControllerStatus, string> _stateEnums = new Dictionary<ServiceControllerStatus, string>
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

        private readonly Dictionary<string, string> _stateStrings = new Dictionary<string, string>
        {
            {"Running", DaemonStates.Running},
            {"Start Pending", DaemonStates.StartPending},
            {"Stopped", DaemonStates.Stopped},
            {"Stop Pending", DaemonStates.StopPending}
        };

        public Win32ServiceManager(IDaemonChangeDistributor daemonChangeDistributor)
        {
            _daemonChangeDistributor = daemonChangeDistributor;
            Initialize();
        }

        private void Initialize()
        {
            if (_daemonDtos.Count > 0)
                return;

            lock (_daemonDtos)
            {
                if (_daemonDtos.Count > 0)
                    return;

                var daemonControllers = _services.Values.Select(serviceController => new
                {
                    // TODO: return DependentServices and ServiceDependsOn, too, these will be required for stopping and starting.
                    DaemonDto = new DaemonDto
                    {
                        DaemonName = serviceController.ServiceName,
                        DisplayName = serviceController.DisplayName,
                        DaemonType = Win32ServiceType,
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

        public Task<IEnumerable<DaemonDto>> FindDaemonsAsync(string contains)
        {
            IEnumerable<DaemonDto> dtos = _daemonDtos;

            if (!string.IsNullOrWhiteSpace(contains))
                dtos = dtos.Where(d =>
                    d.DaemonName.IndexOf(contains, StringComparison.OrdinalIgnoreCase) >= 0
                    || d.DisplayName.IndexOf(contains, StringComparison.OrdinalIgnoreCase) >= 0
                );

            return Task.FromResult(dtos);
        }

        public Task<IEnumerable<DaemonDto>> GetDaemonsAsync(string[] daemonNames)
        {
            var result = _daemonDtos.Where(d => daemonNames.Contains(d.DaemonName));

            return Task.FromResult(result);
        }

        public Task StartAsync(string daemonName)
        {
            if (!_services.TryGetValue(daemonName, out ServiceController serviceController))
                return Task.CompletedTask;

            if (serviceController.Status == ServiceControllerStatus.Stopped)
            {
                Task.Run(() =>
                {
                    _log.InfoFormat("Sending start to {0}", daemonName);
                    serviceController.Start();
                }).ContinueWith(t =>
                {
                    _log.InfoFormat("{0} started.", daemonName);
                    if (t.Status == TaskStatus.Faulted)
                    {
                        // this is usually just due to users double clicking on start/stop
                        // and the servicecontroller doesnt update it's state fast enough.
                        // so we just log a warning and not an error.
                        _log.WarnFormat("Starting {0} failed.", t.Exception, daemonName);
                    }
                });
            }
            else
            {
                _log.Info("Task is not in a state that allowed it to be started.");
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(string daemonName)
        {
            if (!_services.TryGetValue(daemonName, out ServiceController serviceController))
                return Task.CompletedTask;

            if (serviceController.Status == ServiceControllerStatus.Running)
            {
                Task.Run(() =>
                {
                    _log.InfoFormat("Sending stop to {0}", daemonName);
                    serviceController.Stop();
                }).ContinueWith(t =>
                {
                    _log.InfoFormat("{0} stopped.", daemonName);
                    if (t.Status == TaskStatus.Faulted)
                    {
                        // see StartAsync()
                        _log.WarnFormat("Stopped {0} failed.", t.Exception, daemonName);
                    }
                });
            }
            else
            {
                _log.Info("Task is not in a state that allowed it to be stopped.");
            }

            return Task.CompletedTask;
        }

        private void DemoWatcher_EventArrived(object sender, EventArrivedEventArgs eventArgs)
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

            if (_services.TryGetValue(serviceName, out ServiceController serviceController))
                serviceController.Refresh();

            var service = _daemonDtos.FirstOrDefault(s => s.DaemonName == serviceName);
            if (service != null)
                service.State = GetState(state);
            
            _daemonChangeDistributor.Distribute(Win32ServiceType, serviceName, state);
        }

        private string GetState(string status)
        {
            if (_stateStrings.ContainsKey(status))
                return _stateStrings[status];

            return DaemonStates.Unknown;
        }

        private string GetState(ServiceControllerStatus status)
        {
            if (_stateEnums.ContainsKey(status))
                return _stateEnums[status];

            return DaemonStates.Unknown;
        }

        private void LoadDescriptionAsync(ServiceController serviceController, DaemonDto daemonDto)
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
    }
}