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
        // since we subscribe to the changed event, we will require them for the whole lifetime of this process.
        // by staying static, we will just have one instance per service, which is sufficient for us.
        private static readonly ServiceController[] _services = ServiceController.GetServices();

        private static readonly List<DaemonDto> _daemonDtos = new List<DaemonDto>();

        public Win32ServiceManager()
        {
            InitializeDtos();
        }

        private void InitializeDtos()
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
            }
        }

        private void LoadDescriptionAsync(ServiceController serviceController, DaemonDto daemonDto)
        {
            Task.Run(() =>
            {
                try
                {
                    var description = GetDescription(serviceController);
                    daemonDto.Description = description;
                }
                catch (Exception ex)
                {
                    daemonDto.Description = "ERROR: " + ex.Message;
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


        private static string GetDescription(ServiceController service)
        {
            try
            {
                var serviceName = service.ServiceName;
                using (var managementService = new ManagementObject(new ManagementPath($"Win32_Service.Name='{serviceName}'")))
                {
                    return managementService["Description"]?.ToString() ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                return "ERROR reading service Description: " + ex;
            }
        }

        private static string GetState(ServiceControllerStatus status)
        {
            switch (status)
            {
                // may support these later

                //case ServiceControllerStatus.ContinuePending:
                //    break;
                //case ServiceControllerStatus.Paused:
                //    break;
                //case ServiceControllerStatus.PausePending:
                //    break;

                case ServiceControllerStatus.Running:
                    return DaemonStates.Running;

                case ServiceControllerStatus.StartPending:
                    return DaemonStates.Starting;

                case ServiceControllerStatus.Stopped:
                    return DaemonStates.Stopped;

                case ServiceControllerStatus.StopPending:
                    return DaemonStates.Stopping;
            }

            return DaemonStates.Unknown;
        }
    }
}