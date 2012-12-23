using System.Windows;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using MixPlanner.Views;

namespace MixPlanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        IWindsorContainer container;

        protected override void OnStartup(StartupEventArgs e)
        {
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
