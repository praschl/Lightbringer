using Castle.DynamicProxy;
using Common.Logging;
using System;
using System.Collections.Concurrent;
using System.ServiceModel;

namespace MiP.Core.Logging
{
    public class LoggerInterceptor : IInterceptor
    {
        private static readonly ConcurrentDictionary<Type, ILog> _loggers = new ConcurrentDictionary<Type, ILog>();

        public void Intercept(IInvocation invocation)
        {
            var logger = _loggers.GetOrAdd(invocation.TargetType, type => LogManager.GetLogger(type.FullName));

            logger.DebugFormat("BEGIN {0}", invocation.Method.Name);

            try
            {
                invocation.Proceed();

                logger.DebugFormat("END {0}", invocation.Method.Name);
            }
            catch (FaultException ex)
            {
                logger.ErrorFormat("EXCEPTION EXIT {0}", ex, invocation.Method.Name);
                throw;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("EXCEPTION EXIT {0}", ex, invocation.Method.Name);
                throw;
            }
        }
    }
}
