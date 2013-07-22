using System;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    public class DropItemIntoAudioPlayerCommand : CommandBase<IDropInfo>
    {
        readonly PlayPauseTrackCommand playPauseTrackCommand;

        public DropItemIntoAudioPlayerCommand(
            PlayPauseTrackCommand playPauseTrackCommand)
        {
            if (playPauseTrackCommand == null) throw new ArgumentNullException("playPauseTrackCommand");
            this.playPauseTrackCommand = playPauseTrackCommand;
        }

        protected override bool CanExecute(IDropInfo parameter)
        {
            Track track = parameter.GetTrack();
            return playPauseTrackCommand.CanExecute(track);
        }

        protected override void Execute(IDropInfo parameter)
        {
            Track track = parameter.GetTrack();
            playPauseTrackCommand.Execute(track);
        }
    }
}