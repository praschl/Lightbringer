using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Lightbringer.Wcf.ServiceModules
{
    public class NLogBehavior : IServiceBehavior
    {
        private readonly Type _serviceType;

        public NLogBehavior(Type serviceType)
        {
            _serviceType = serviceType;
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            // LNogErrorHandler zum ServiceHost hinzufügen.

            foreach (var dispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                var channelDispatcher = dispatcherBase as ChannelDispatcher;
                channelDispatcher?.ErrorHandlers.Add(new NLogErrorHandler(_serviceType));
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }
}