﻿using System;
using System.Windows.Input;

namespace MixPlanner.Commands
{
    public class CloseSettingsCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged = delegate { };
    }
}