namespace MixPlanner.Commands
{
    using GalaSoft.MvvmLight.Messaging;
    using MixPlanner.DomainModel;
    using MixPlanner.Player;

    public sealed class PlayNextTrackCommand : PlaylistCommandBase
    {
        public PlayNextTrackCommand(IPlaylist playlist, IMessenger messenger, IAudioPlayer player)
            : base(playlist, messenger, player, p => p.NextTrack)
        {
        }
    }
}