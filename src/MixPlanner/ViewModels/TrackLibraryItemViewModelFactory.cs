using System;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Commands;
using MixPlanner.DomainModel;
using MixPlanner.Player;

namespace MixPlanner.ViewModels
{
    public interface ITrackLibraryItemViewModelFactory
    {
        TrackLibraryItemViewModel CreateViewModel(Track track);
    }

    public class TrackLibraryItemViewModelFactory : ITrackLibraryItemViewModelFactory
    {
        readonly IMessenger messenger;
        readonly IAudioPlayer player;
        readonly QuickEditBpmCommand quickEditBpmCommand;
        readonly QuickEditHarmonicKeyCommand quickEditHarmonicKeyCommand;
        readonly PlayPauseTrackCommand playPauseCommand;

        public TrackLibraryItemViewModelFactory(
            IMessenger messenger, 
            IAudioPlayer player, 
            QuickEditBpmCommand quickEditBpmCommand, 
            QuickEditHarmonicKeyCommand quickEditHarmonicKeyCommand, 
            PlayPauseTrackCommand playPauseCommand)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (player == null) throw new ArgumentNullException("player");
            if (quickEditBpmCommand == null) throw new ArgumentNullException("quickEditBpmCommand");
            if (quickEditHarmonicKeyCommand == null) throw new ArgumentNullException("quickEditHarmonicKeyCommand");
            if (playPauseCommand == null) throw new ArgumentNullException("playPauseCommand");
            this.messenger = messenger;
            this.player = player;
            this.quickEditBpmCommand = quickEditBpmCommand;
            this.quickEditHarmonicKeyCommand = quickEditHarmonicKeyCommand;
            this.playPauseCommand = playPauseCommand;
        }

        public TrackLibraryItemViewModel CreateViewModel(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            return new TrackLibraryItemViewModel(messenger, player, track, quickEditBpmCommand, quickEditHarmonicKeyCommand, playPauseCommand);
        }
    }
}