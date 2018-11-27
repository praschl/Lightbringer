using System.Collections.Generic;
using System.ServiceProcess;

namespace MiP.Core.Services
{
    public class CoreService : ServiceBase
    {
        private readonly IEnumerable<IServiceModule> _serviceModules;

        public CoreService(IEnumerable<IServiceModule> serviceModules)
        {
            _serviceModules = serviceModules;
        }

        protected override void OnStart(string[] args)
        {
            foreach (var serviceModule in _serviceModules)
            {
                serviceModule.Open();
            }
        }

        protected override void OnStop()
        {
            foreach (var serviceModule in _serviceModules)
            {
                serviceModule.Close();
            }
        }

        public void Start(string[] args)
        {
            OnStart(args);
        }
    }
}
