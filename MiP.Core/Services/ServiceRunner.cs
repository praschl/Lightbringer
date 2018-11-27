using Common.Logging;
using System;
using System.Configuration.Install;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;

namespace MiP.Core.Services
{
    public static class ServiceRunner
    {
        private static ILog _log = LogManager.GetLogger(typeof(ServiceRunner));

        public static void Run(IServiceProvider serviceProvider, string[] args = null)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            // service host better runs on invariant culture.
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            AppDomain.CurrentDomain.UnhandledException += GlobalUnhandledExceptionHandler;

            RunService(serviceProvider, args);
        }

        private static void GlobalUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            _log.Fatal("An unhandled exception was catched.", e.ExceptionObject as Exception);
        }

        private static void RunService(IServiceProvider serviceProvider, string[] args)
        {
            args = args ?? new string[0];

            if (Environment.UserInteractive)
            {
                if (args.Contains("-i", StringComparer.OrdinalIgnoreCase))
                    TryInstallService();
                else if (args.Contains("-u", StringComparer.OrdinalIgnoreCase))
                    TryUninstallService();
                else if (args.Contains("-initialize", StringComparer.OrdinalIgnoreCase))
                    Initialize(serviceProvider);
                else
                    RunInteractive(serviceProvider, args);
            }
            else
            {
                RunAsService(serviceProvider);
            }
        }
        
        private static void RunAsService(IServiceProvider serviceProvider)
        {
            var service = ResolveCoreService(serviceProvider);

            var servicesToRun = new ServiceBase[]
            {
                service
            };

            ServiceBase.Run(servicesToRun);
        }

        private static void RunInteractive(IServiceProvider serviceProvider, string[] args)
        {
            try
            {
                Console.WriteLine("Running interactive, to install as a windows service, use \"-i\", to uninstall use \"-u\", and make sure you have the privileges to install a windows service.");

                var service = ResolveCoreService(serviceProvider);

                service.Start(args);

                WaitForEscape(); // this could be made into a REPL

                service.Stop();
            }
            catch (Exception ex)
            {
                _log.Fatal("Fatal error while running interactive.", ex);
                WaitForEscape();
            }
        }

        private static CoreService ResolveCoreService(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<CoreService>();
        }

        private static void TryInstallService()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new[] { Assembly.GetEntryAssembly().Location });
            }
            catch (Exception ex)
            {
                _log.Fatal("Fatal error while trying to install service.", ex);

                Console.WriteLine("Make sure you have the privileges to install the service, try running this command as Administrator");
            }
        }

        private static void TryUninstallService()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new[] { "/u", Assembly.GetEntryAssembly().Location });
            }
            catch (Exception ex)
            {
                _log.Fatal("Fatal error while trying to uninstall service.", ex);

                Console.WriteLine("Make sure you have the privileges to install the service, try running this command as Administrator");
            }
        }

        private static void Initialize(IServiceProvider serviceProvider)
        {
            try
            {
                serviceProvider.GetService<IInitializer>()
                    .Initialize();
            }
            catch (Exception ex)
            {
                _log.Fatal("Fatal error while initializing service.", ex);
            }
        }

        private static void WaitForEscape()
        {
            Console.WriteLine("Press ESC to stop service...");
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                    break;
            }
        }
    }
}
