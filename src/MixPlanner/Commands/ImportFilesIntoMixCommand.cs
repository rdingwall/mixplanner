using System;
using System.Collections.Generic;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.DomainModel;
using MixPlanner.ProgressDialog;

namespace MixPlanner.Commands
{
    public class ImportFilesIntoMixCommand : ImportFilesCommandBase
    {
        readonly IMix mix;

        public ImportFilesIntoMixCommand(ITrackLibrary library, IMix mix,
            IProgressDialogService progressDialog) : base(library, progressDialog)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            this.mix = mix;
        }

        protected override bool CanExecute()
        {
            return !mix.IsLocked;
        }

        protected override void OnImported(IEnumerable<Track> tracks, IDropInfo dropInfo)
        {
            mix.Insert(tracks, dropInfo.InsertIndex);
        }
    }
}