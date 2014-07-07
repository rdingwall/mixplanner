namespace MixPlanner.Commands
{
    using System;
    using MixPlanner.Events;

    public sealed class SearchLibraryCommand : CommandBase<string>
    {
        private readonly IDispatcherMessenger messenger;

        public SearchLibraryCommand(IDispatcherMessenger messenger)
        {
            if (messenger == null)
            {
                throw new ArgumentNullException("messenger");
            }
            this.messenger = messenger;
        }

        protected override void Execute(string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                messenger.SendToUI(new SearchTextClearedEvent());
            }
            else
            {
                messenger.Send(new SearchRequestedEvent(parameter));
            }
        }
    }
}