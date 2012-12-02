using System;
using System.Windows.Input;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    public class RemoveTrackFromMixCommand : ICommand
    {
        readonly IMix mix;

        public RemoveTrackFromMixCommand(IMix mix)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            this.mix = mix;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter == null) return;

            var mixItem = (MixItem) parameter;
            mix.Remove(mixItem);
        }

        public event EventHandler CanExecuteChanged;
    }
}