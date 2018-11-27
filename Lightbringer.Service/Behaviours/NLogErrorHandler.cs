using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using NLog;

namespace Lightbringer.Service.Behaviours
{
    public class NLogErrorHandler : IErrorHandler
    {
        // more info about IErrorHandler and behaviours at https://blogs.msdn.microsoft.com/carlosfigueira/2011/06/07/wcf-extensibility-ierrorhandler/

        private readonly Logger _logger;

        public NLogErrorHandler(Type serviceType)
        {
            // logger für den Service erstellen.
            _logger = LogManager.GetLogger(serviceType.FullName);
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
        }

        public bool HandleError(Exception error)
        {
            if (error is FaultException)
            {
                // FaultException entspricht einem Benutzer Fehler, die direkt vom Service geworfen wird,
                // daher auch bewusst auftreten kann, deswegen hier nur Info-log, Benutzerfehler sind im log
                // wesentlich unwichtiger als Programm Fehler.

                _logger.Info(error);
                return false;
            }

            // alle anderen sind unbekannt, und sollten entsprechend gelogged werden.
            _logger.Error(error, "An unhandled error happened while dispatching a request.");

            return false;
        }
    }
}