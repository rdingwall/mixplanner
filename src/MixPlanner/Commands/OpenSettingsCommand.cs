using System;
using System.Windows.Input;
using Castle.Windsor;
using MixPlanner.Views;

namespace MixPlanner.Commands
{
    public class OpenSettingsCommand : ICommand
    {
        readonly IWindsorContainer container;

        public OpenSettingsCommand(IWindsorContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            this.container = container;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            container.Resolve<SettingsWindow>().ShowDialog();
        }

        public event EventHandler CanExecuteChanged = delegate { };
    }
}