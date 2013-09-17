using System;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Player;

namespace MixPlanner.DomainModel
{
    public interface IPlaylist
    {
        Track PreviousTrack { get; }
        Track NextTrack { get; }
    }

    public class Playlist : IPlaylist
    {
        readonly IAudioPlayer player;
        readonly ICurrentMixProvider mixProvider;

        public Playlist(IAudioPlayer player, ICurrentMixProvider mixProvider, IMessenger messenger)
        {
            if (mixProvider == null) throw new ArgumentNullException("mixProvider");
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.player = player;
            this.mixProvider = mixProvider;
        }

        public Track PreviousTrack
        {
            get
            {
                if (player.CurrentTrack == null)
                    return null;

                IMix mix = mixProvider.GetCurrentMix();

                if (mix.IsEmpty)
                    return null;

                int currentIndex = mix.IndexOf(player.CurrentTrack);
                if (currentIndex == -1)
                    return null;

                int previousIndex = Math.Min(currentIndex - 1, mix.Count - 1);

                if (previousIndex < 0)
                    return null;

                return mix[previousIndex].Track;
            }
        }

        public Track NextTrack
        {
            get
            {
                if (player.CurrentTrack == null)
                    return null;

                IMix mix = mixProvider.GetCurrentMix();

                if (mix.IsEmpty)
                    return null;

                int currentIndex = mix.IndexOf(player.CurrentTrack);
                if (currentIndex == -1)
                    return null;

                int nextIndex = currentIndex + 1;

                if (nextIndex >= mix.Count)
                    return null;

                return mix[nextIndex].Track;
            }
        }
    }
}