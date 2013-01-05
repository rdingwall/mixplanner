using System;
using System.Threading.Tasks;
using Castle.Windsor;
using MixPlanner.Storage;
using MixPlanner.ViewModels;
using MixPlanner.Views;

namespace MixPlanner.Commands
{
    public class OpenSettingsCommand : AsyncCommandBase
    {
        readonly IConfigurationStorage configurationStorage;
        readonly IWindsorContainer container;

        public OpenSettingsCommand(
            IConfigurationStorage configurationStorage,
            IWindsorContainer container)
        {
            if (configurationStorage == null) throw new ArgumentNullException("configurationStorage");
            if (container == null) throw new ArgumentNullException("container");
            this.configurationStorage = configurationStorage;
            this.container = container;
        }

        protected override async Task DoExecute(object parameter)
        {
            var configuration = await configurationStorage.GetConfiguration();

            var window = container.Resolve<SettingsWindow>();
            var viewModel = container.Resolve<SettingsWindowViewModel>();
            window.DataContext = viewModel;
            viewModel.Initialize(configuration);
            window.ShowDialog();
        }
    }
}