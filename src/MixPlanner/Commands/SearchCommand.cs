using System;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Events;

namespace MixPlanner.Commands
{
    public class SearchCommand : CommandBase<string>
    {
        readonly IMessenger messenger;

        public SearchCommand(IMessenger messenger)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.messenger = messenger;
        }

        protected override void Execute(string parameter)
        {
            if (String.IsNullOrWhiteSpace(parameter))
                messenger.Send(new SearchTextClearedEvent());
            else
                messenger.Send(new SearchRequestedEvent(parameter));
        }
    }
}