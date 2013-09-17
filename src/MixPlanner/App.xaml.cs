using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Castle.Windsor;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Practices.ServiceLocation;
using MixPlanner.Configuration;
using MixPlanner.DomainModel;
using MixPlanner.Storage;
using MixPlanner.Views;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;

namespace MixPlanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static readonly ILog Log = LogManager.GetLogger(typeof (App));
        IWindsorContainer container;

        static readonly string version =
                Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public App()
        {
            DispatcherUnhandledException += OnAppDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainOnUnhandledException;
        }

        public static string Version { get { return version; } }

        protected override void OnStartup(StartupEventArgs e)
        {
            LoadTheme();
            ConfigureLog4Net(e);
            DispatcherHelper.Initialize();
            container = new WindsorContainer();
            container.Install(new MixPlannerWindsorInstaller());
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));

            Task.Run(() => container.Resolve<IConfigProvider>().InitializeAsync()).Wait();
            Task.Run(() => container.Resolve<ITrackLibrary>().InitializeAsync()).Wait();

            Log.DebugFormat("MixPlanner {0} starting up", Assembly.GetExecutingAssembly().GetName().Version);

            container.Resolve<MainWindow>().ShowDialog();
        }

        static void ConfigureLog4Net(StartupEventArgs e)
        {
            var appenders = new List<IAppender>();

            var consoleAppender = new ConsoleAppender
                                      {
                                          Layout = new PatternLayout("%date [%thread] %level %logger %ndc - %message%newline")
                                      };
            consoleAppender.ActivateOptions();
            appenders.Add(consoleAppender);

            var errorFileAppender = new RollingFileAppender
            {
                File = MixPlannerPaths.ErrorLogFile,
                Layout = new PatternLayout("%date [%thread] %level %logger %ndc - %message%newline"),
                Threshold = Level.Warn,
                LockingModel = new FileAppender.MinimalLock(),
                MaxSizeRollBackups = 10, // number of files
                MaximumFileSize = "10MB",
                StaticLogFileName = true // always write to the latest
            };
            errorFileAppender.ActivateOptions();
            appenders.Add(errorFileAppender);

            if (e.Args.Contains("/debug", StringComparer.CurrentCultureIgnoreCase))
            {
                var debugFileAppender = new RollingFileAppender
                {
                    File = MixPlannerPaths.DebugLogFile,
                    Layout = new PatternLayout("%date [%thread] %level %logger %ndc - %message%newline"),
                    Threshold = Level.Debug,
                    LockingModel = new FileAppender.MinimalLock(),
                    MaxSizeRollBackups = 10, // number of files
                    MaximumFileSize = "10MB",
                    StaticLogFileName = true // always write to the latest
                };
                debugFileAppender.ActivateOptions();
                appenders.Add(debugFileAppender);
            }

            BasicConfigurator.Configure(appenders.ToArray());
        }

        void LoadTheme()
        {
            var themeResources = (ResourceDictionary) LoadComponent(new Uri(@"Themes\MixPlanner.xaml", UriKind.Relative));
            Resources.MergedDictionaries.Add(themeResources);
        }

        static void OnAppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Error(e.Exception);
            e.Handled = true;
        }

        static void OnCurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error(e.ExceptionObject);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            container.Dispose();
        }
    }
}
