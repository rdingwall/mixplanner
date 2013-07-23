using System;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    public class DropItemIntoAudioPlayerCommand : CommandBase<IDropInfo>
    {
        readonly PlayPauseTrackCommand playPauseTrackCommand;
        readonly ImportFilesAndPlayCommand importFilesCommand;

        public DropItemIntoAudioPlayerCommand(
            PlayPauseTrackCommand playPauseTrackCommand, 
            ImportFilesAndPlayCommand importFilesCommand)
        {
            if (playPauseTrackCommand == null) throw new ArgumentNullException("playPauseTrackCommand");
            if (importFilesCommand == null) throw new ArgumentNullException("importFilesCommand");
            this.playPauseTrackCommand = playPauseTrackCommand;
            this.importFilesCommand = importFilesCommand;
        }

        protected override bool CanExecute(IDropInfo parameter)
        {
            return playPauseTrackCommand.CanExecute(parameter.GetTrack())
                || importFilesCommand.CanExecute(parameter);
        }

        protected override void Execute(IDropInfo parameter)
        {
            Track track = parameter.GetTrack();
            if (track != null)
            {
                playPauseTrackCommand.Execute(track);
                return;
            }

            importFilesCommand.Execute(parameter);
        }
    }
}