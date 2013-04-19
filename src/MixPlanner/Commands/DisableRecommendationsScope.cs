using System;
using MixPlanner.Events;

namespace MixPlanner.Commands
{
    public class DisableRecommendationsScope : IDisposable
    {
        readonly IDispatcherMessenger messenger;

        public DisableRecommendationsScope(IDispatcherMessenger messenger)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.messenger = messenger;
            messenger.SendToUI(new RecommendationsDisabledEvent());
        }

        public void Dispose()
        {
            messenger.SendToUI(new RecommendationsEnabledEvent());
        }
    }
}