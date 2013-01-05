using System;
using System.Windows.Input;
using MixPlanner.Views;

namespace MixPlanner.Commands
{
    public class OpenSettingsCommand : ICommand
    {
        readonly SettingsWindow window;

        public OpenSettingsCommand(SettingsWindow window)
        {
            if (window == null) throw new ArgumentNullException("window");
            this.window = window;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            window.ShowDialog();
        }

        public event EventHandler CanExecuteChanged = delegate { };
    }
}