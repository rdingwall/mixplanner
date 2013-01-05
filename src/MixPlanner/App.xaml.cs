using System.Windows;
using System.Windows.Threading;
using Castle.Windsor;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Practices.ServiceLocation;
using MixPlanner.Views;
using log4net;
using log4net.Config;

namespace MixPlanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static readonly ILog Log = LogManager.GetLogger(typeof (App));
        IWindsorContainer container;

        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Error(e.Exception);
            e.Handled = true;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            BasicConfigurator.Configure();
            DispatcherHelper.Initialize();
            container = new WindsorContainer();
            container.Install(new IocRegistrations());
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
            container.Resolve<MainWindow>().ShowDialog();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            container.Dispose();
        }
    }
}
