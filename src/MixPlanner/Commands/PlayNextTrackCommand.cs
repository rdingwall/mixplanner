using GalaSoft.MvvmLight.Messaging;
using MixPlanner.DomainModel;
using MixPlanner.Player;

namespace MixPlanner.Commands
{
    public class PlayNextTrackCommand : PlaylistCommandBase
    {
        public PlayNextTrackCommand(IPlaylist playlist, IMessenger messenger, IAudioPlayer player)
            : base(playlist, messenger, player, p => p.NextTrack)
        {}
    }
}