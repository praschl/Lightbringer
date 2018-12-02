using System;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Web.Http;
using Lightbringer.Rest.Contract;

namespace Lightbringer.WebApi
{
    public class DaemonsController : ApiController, IDaemonApi
    {
        public Task<DaemonDto[]> GetAllDaemonsAsync()
        {
            // TODO: code in eigenes Projekt Win32 schieben

            var services = ServiceController.GetServices();

            var dtos = services.Select(s => new DaemonDto
            {
                ServiceName = s.ServiceName,
                DisplayName = s.DisplayName,
                Description = "", // GetDescription(s.ServiceName),
                State = GetState(s.Status)
            }).ToArray();

            return Task.FromResult(dtos);
        }

        private string GetDescription(string serviceName)
        {
            try
            {
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

        private string GetState(ServiceControllerStatus status)
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