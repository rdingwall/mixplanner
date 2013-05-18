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
        readonly IConfigStorage configStorage;
        readonly IWindsorContainer container;

        public OpenSettingsCommand(
            IConfigStorage configStorage,
            IWindsorContainer container)
        {
            if (configStorage == null) throw new ArgumentNullException("configStorage");
            if (container == null) throw new ArgumentNullException("container");
            this.configStorage = configStorage;
            this.container = container;
        }

        protected override async Task DoExecute(object parameter)
        {
            var config = await configStorage.LoadConfigAsync();

            var window = container.Resolve<SettingsWindow>();
            var viewModel = container.Resolve<SettingsWindowViewModel>();
            window.DataContext = viewModel;
            viewModel.Initialize(config);
            window.ShowDialog();
        }
    }
}