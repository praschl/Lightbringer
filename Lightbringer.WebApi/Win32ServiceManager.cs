using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Threading.Tasks;
using Lightbringer.Rest.Contract;

namespace Lightbringer.WebApi
{
    public static class Win32ServiceManager
    {
        // we make this static, because all those service controllers should be disposed when no longer needed
        // since we subscribe to the changed event, we will require them for the whole lifetime of this process.
        // by staying static, we will just have one instance per service, which is sufficient for us.
        private static readonly ServiceController[] _services = ServiceController.GetServices();
        private static readonly Dictionary<string,string> _serviceDescriptions = new Dictionary<string, string>();

        public static async Task InitializeAsync()
        {
            await Task.WhenAll(
                _services.Select(LoadDescriptionAsync).ToArray()
            );
        }

        public static Task<DaemonDto[]> GetDaemons(string contains)
        {
            var dtos = _services.Select(s => new DaemonDto
            {
                ServiceName = s.ServiceName,
                DisplayName = s.DisplayName,
                ServiceType = "Win32 Service",
                Description = _serviceDescriptions.ContainsKey(s.ServiceName) ? _serviceDescriptions[s.ServiceName] : null,
                State = GetState(s.Status)
            });

            if (!string.IsNullOrWhiteSpace(contains))
                dtos = dtos.Where(d =>
                    d.ServiceName.IndexOf(contains, StringComparison.OrdinalIgnoreCase) >= 0
                    || d.DisplayName.IndexOf(contains, StringComparison.OrdinalIgnoreCase) >= 0
                );

            return Task.FromResult(dtos.ToArray());
        }

        private static Task LoadDescriptionAsync(ServiceController service)
        {
            return Task.Run(() =>
            {
                var description = GetDescription(service);
                _serviceDescriptions[service.ServiceName] = description;
            });
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