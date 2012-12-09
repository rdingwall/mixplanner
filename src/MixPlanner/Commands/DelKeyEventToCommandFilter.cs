using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace MixPlanner.Commands
{
    public class DelKeyEventToCommandFilter : CommandBase<KeyEventArgs>
    {
        readonly IEnumerable<Key> deleteKeys = new[] { Key.Delete, Key.Back };
        readonly ICommand command;
        readonly Func<object> getParameter;

        public DelKeyEventToCommandFilter(ICommand command, Func<object> getParameter)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (getParameter == null) throw new ArgumentNullException("getParameter");
            this.command = command;
            this.getParameter = getParameter;
        }

        protected override void DoExecute(KeyEventArgs parameter)
        {
            if (!deleteKeys.Contains(parameter.Key))
                return;

            var commandParameter = getParameter();
            if (command.CanExecute(commandParameter))
                command.Execute(commandParameter);
        }
    }
}