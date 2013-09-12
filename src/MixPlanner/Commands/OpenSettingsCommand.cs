using System;
using System.Threading.Tasks;
using Castle.Windsor;
using MixPlanner.Configuration;
using MixPlanner.ViewModels;
using MixPlanner.Views;

namespace MixPlanner.Commands
{
    public class OpenSettingsCommand : AsyncCommandBase
    {
        readonly IConfigProvider configProvider;
        readonly IWindsorContainer container;

        public OpenSettingsCommand(
            IConfigProvider configProvider,
            IWindsorContainer container)
        {
            if (configProvider == null) throw new ArgumentNullException("configProvider");
            if (container == null) throw new ArgumentNullException("container");
            this.configProvider = configProvider;
            this.container = container;
        }

        protected override async Task DoExecute(object parameter)
        {
            var config = configProvider.Config;

            var window = container.Resolve<SettingsWindow>();
            var viewModel = container.Resolve<SettingsWindowViewModel>();
            window.DataContext = viewModel;
            viewModel.Initialize(config);
            window.ShowDialog();
        }
    }
}