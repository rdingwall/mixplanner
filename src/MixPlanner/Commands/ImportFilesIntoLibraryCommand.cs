using System;
using System.Windows;
using MixPlanner.DomainModel;
using MixPlanner.ProgressDialog;

namespace MixPlanner.Commands
{
    public class ImportFilesIntoLibraryCommand : CommandBase<DragEventArgs>
    {
        readonly ITrackLibrary library;
        readonly IProgressDialogService progressDialog;

        static readonly ProgressDialogOptions ProgressDialogOptions
            = new ProgressDialogOptions
            {
                Label = "Importing tracks",
                WindowTitle = "Importing tracks"
            };

        public ImportFilesIntoLibraryCommand(ITrackLibrary library,
            IProgressDialogService progressDialog)
        {
            if (library == null) throw new ArgumentNullException("library");
            if (progressDialog == null) throw new ArgumentNullException("progressDialog");
            this.library = library;
            this.progressDialog = progressDialog;
        }

        protected override bool CanExecute(DragEventArgs parameter)
        {
            return parameter != null;
        }

        protected override void Execute(DragEventArgs parameter)
        {
            var filenames = (string[]) parameter.Data.GetData(DataFormats.FileDrop);

            if (filenames == null) return;

            progressDialog.ExecuteAsync(
                async (token, progress) => await library.ImportAsync(filenames, token, progress),
                ProgressDialogOptions);
        }
    }
}