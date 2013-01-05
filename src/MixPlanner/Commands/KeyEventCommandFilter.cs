using System;
using System.Linq;
using System.Windows.Input;

namespace MixPlanner.Commands
{
    public class KeyEventCommandFilter : CommandBase<KeyEventArgs>
    {
        readonly ICommand command;
        readonly Func<object> getParameter;
        readonly Key[] keys;

        public KeyEventCommandFilter(ICommand command, Func<object> getParameter, params Key[] keys)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (getParameter == null) throw new ArgumentNullException("getParameter");
            if (keys == null) throw new ArgumentNullException("keys");
            this.command = command;
            this.getParameter = getParameter;
            this.keys = keys;
        }

        protected override bool CanExecute(KeyEventArgs parameter)
        {
            return parameter != null && keys.Contains(parameter.Key);
        }

        protected override void Execute(KeyEventArgs parameter)
        {
            if (!CanExecute(parameter))
            {
                parameter.Handled = false;
                return;
            }

            var commandParameter = getParameter();
            if (command.CanExecute(commandParameter))
                command.Execute(commandParameter);

            parameter.Handled = true;
        }
    }
}