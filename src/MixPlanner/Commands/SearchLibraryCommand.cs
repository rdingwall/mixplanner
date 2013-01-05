using System;
using MixPlanner.Events;

namespace MixPlanner.Commands
{
    public class SearchLibraryCommand : CommandBase<string>
    {
        readonly IDispatcherMessenger messenger;

        public SearchLibraryCommand(IDispatcherMessenger messenger)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.messenger = messenger;
        }

        protected override void Execute(string parameter)
        {
            if (String.IsNullOrWhiteSpace(parameter))
                messenger.SendToUI(new SearchTextClearedEvent());
            else
                messenger.Send(new SearchRequestedEvent(parameter));
        }
    }
}