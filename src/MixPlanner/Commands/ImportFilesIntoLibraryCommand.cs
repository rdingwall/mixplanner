using System;
using System.Threading.Tasks;
using System.Windows;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    public class ImportFilesIntoLibraryCommand : AsyncCommandBase<DragEventArgs>
    {
        readonly ITrackLibrary library;

        public ImportFilesIntoLibraryCommand(ITrackLibrary library)
        {
            if (library == null) throw new ArgumentNullException("library");
            this.library = library;
        }

        protected override bool CanExecute(DragEventArgs parameter)
        {
            return parameter != null;
        }

        protected async override Task DoExecute(DragEventArgs parameter)
        {
            var filenames = (string[]) parameter.Data.GetData(DataFormats.FileDrop);

            if (filenames == null) return;

            await library.ImportAsync(filenames);
        }
    }
}