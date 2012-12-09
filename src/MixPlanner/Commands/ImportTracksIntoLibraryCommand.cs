using System;
using System.Windows;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    public class ImportTracksIntoLibraryCommand : CommandBase<DragEventArgs>
    {
        readonly ITrackLibrary library;

        public ImportTracksIntoLibraryCommand(ITrackLibrary library)
        {
            if (library == null) throw new ArgumentNullException("library");
            this.library = library;
        }

        protected override bool DoCanExecute(DragEventArgs parameter)
        {
            return parameter != null;
        }

        protected override void DoExecute(DragEventArgs parameter)
        {
            var filenames = (string[]) parameter.Data.GetData(DataFormats.FileDrop);

            if (filenames == null) return;

            library.Import(filenames);
        }
    }
}