namespace MixPlanner.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GongSolutions.Wpf.DragDrop;
    using MixPlanner.DomainModel;
    using MixPlanner.ProgressDialog;

    public sealed class ImportFilesAndPlayCommand : ImportFilesCommandBase
    {
        private readonly PlayPauseTrackCommand playCommand;

        public ImportFilesAndPlayCommand(
            ITrackLibrary library, 
            IProgressDialogService progressDialog,
            PlayPauseTrackCommand playCommand) : base(library, progressDialog)
        {
            if (playCommand == null)
            {
                throw new ArgumentNullException("playCommand");
            }

            this.playCommand = playCommand;
        }

        protected override void OnImported(IEnumerable<Track> tracks, IDropInfo dropInfo)
        {
            Track track = tracks.First();
            if (playCommand.CanExecute(track))
            {
                playCommand.Execute(track);
            }
        }
    }
}