﻿using GalaSoft.MvvmLight.Messaging;
using MixPlanner.DomainModel;
using MixPlanner.Player;

namespace MixPlanner.Commands
{
    public class PlayPreviousTrackCommand : PlaylistCommandBase
    {
        public PlayPreviousTrackCommand(IPlaylist playlist, IMessenger messenger, IAudioPlayer player)
            : base(playlist, messenger, player, p => p.PreviousTrack)
        { }
    }
}