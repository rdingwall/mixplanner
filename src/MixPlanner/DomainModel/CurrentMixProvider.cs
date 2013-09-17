using System;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Events;

namespace MixPlanner.DomainModel
{
    public interface ICurrentMixProvider
    {
        IMix GetCurrentMix();
    }

    public class CurrentMixProvider : ICurrentMixProvider
    {
        IMix currentMix;

        public CurrentMixProvider(IMessenger messenger)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");

            messenger.Register<MixLoadedEvent>(this, OnMixLoaded);
        }

        void OnMixLoaded(MixLoadedEvent obj)
        {
            currentMix = obj.Mix;
        }

        public IMix GetCurrentMix()
        {
            if (currentMix == null)
                throw new InvalidOperationException("Attempt to access mix but no mix was loaded yet.");

            return currentMix;
        }
    }
}