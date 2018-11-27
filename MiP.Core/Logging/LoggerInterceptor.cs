using Castle.DynamicProxy;
using System;
using System.Collections.Concurrent;
using System.ServiceModel;
using NLog;

namespace MiP.Core.Logging
{
    public class LoggerInterceptor : IInterceptor
    {
        // thread-safe dictionary für die logger unterschiedlicher klassen.
        private static readonly ConcurrentDictionary<Type, ILogger> Loggers = new ConcurrentDictionary<Type, ILogger>();

        public void Intercept(IInvocation invocation)
        {
            // wird aufgerufen, wenn eine methode einer klasse, die diesen interceptor verwendet aufgerufen wird.

            var logger = Loggers.GetOrAdd(invocation.TargetType, type => LogManager.GetLogger(type.FullName));

            logger.Debug("BEGIN {0}", invocation.Method.Name);

            try
            {
                // die eigentliche Methode der originalen aufrufen, die ohne den Interceptor direkt aufgerufen worden wäre.
                invocation.Proceed();

                logger.Debug("END {0}", invocation.Method.Name);
            }
            catch (FaultException ex)
            {
                // FaultExceptions sollten nur noch auf Service-Ebene vorkommen, und bedeuten, dass ein User-Fehler vorlag...
                // zB schlechte Eingabe.

                logger.Info(ex, "EXCEPTION EXIT {0}", invocation.Method.Name);
                throw;
            }
            catch (Exception ex)
            {
                logger.Warn(ex, "EXCEPTION EXIT {0}", invocation.Method.Name);
                throw;
            }
        }
    }
}
