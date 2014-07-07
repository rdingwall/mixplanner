﻿namespace MixPlanner.Commands
{
    using System;
    using System.Windows.Input;
    using MixPlanner.Events;

    public sealed class ClearRecommendationsCommand : ICommand
    {
        private readonly IDispatcherMessenger messenger;

        public ClearRecommendationsCommand(IDispatcherMessenger messenger)
        {
            if (messenger == null)
            {
                throw new ArgumentNullException("messenger");
            }

            this.messenger = messenger;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            messenger.SendToUI(new RecommendationsClearedEvent());
        }
    }
}