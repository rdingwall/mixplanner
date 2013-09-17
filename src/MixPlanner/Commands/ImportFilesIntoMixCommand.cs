using System;
using System.Collections.Generic;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.DomainModel;
using MixPlanner.ProgressDialog;

namespace MixPlanner.Commands
{
    public class ImportFilesIntoMixCommand : ImportFilesCommandBase
    {
        readonly ICurrentMixProvider mixProvider;

        public ImportFilesIntoMixCommand(ITrackLibrary library, ICurrentMixProvider mixProvider,
            IProgressDialogService progressDialog) : base(library, progressDialog)
        {
            if (mixProvider == null) throw new ArgumentNullException("mixProvider");
            this.mixProvider = mixProvider;
        }

        protected override bool CanExecute()
        {
            IMix mix = mixProvider.GetCurrentMix();
            return !mix.IsLocked;
        }

        protected override void OnImported(IEnumerable<Track> tracks, IDropInfo dropInfo)
        {
            IMix mix = mixProvider.GetCurrentMix();
            mix.Insert(tracks, dropInfo.InsertIndex);
        }
    }
}