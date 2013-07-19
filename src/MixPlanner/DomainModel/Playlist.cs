using System;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Events;
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
        readonly IMix mix;

        public Playlist(IAudioPlayer player, IMix mix, IMessenger messenger)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.player = player;
            this.mix = mix;
        }

        public Track PreviousTrack
        {
            get
            {
                if (player.CurrentTrack == null)
                    return null;

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