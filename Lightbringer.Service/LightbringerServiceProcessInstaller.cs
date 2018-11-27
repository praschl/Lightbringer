using System.ComponentModel;
using System.ServiceProcess;

namespace Lightbringer.Service
{
    [RunInstaller(true)]
    public class LightbringerServiceProcessInstaller : ServiceProcessInstaller
    {
        public const string ServiceName = "LightbringerServiceHost";

        public LightbringerServiceProcessInstaller()
        {
            Account = ServiceAccount.NetworkService;

            Installers.Add(new ServiceInstaller
            {
                ServiceName = ServiceName,
                DisplayName = "Lightbringer Service",
                Description = "Service for the Lightbringer.",
                StartType = ServiceStartMode.Automatic
            });
        }
    }
}
