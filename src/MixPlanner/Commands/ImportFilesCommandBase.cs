namespace MixPlanner.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using GongSolutions.Wpf.DragDrop;
    using MixPlanner.DomainModel;
    using MixPlanner.ProgressDialog;

    public abstract class ImportFilesCommandBase : CommandBase<DropInfo>
    {
        private static readonly ProgressDialogOptions ProgressDialogOptions
            = new ProgressDialogOptions
                  {
                      Label = "Importing tracks",
                      WindowTitle = "Importing tracks"
                  };

        private readonly ITrackLibrary library;
        private readonly IProgressDialogService progressDialog;

        protected ImportFilesCommandBase(ITrackLibrary library, IProgressDialogService progressDialog)
        {
            if (library == null)
            {
                throw new ArgumentNullException("library");
            }

            if (progressDialog == null)
            {
                throw new ArgumentNullException("progressDialog");
            }

            this.library = library;
            this.progressDialog = progressDialog;
        }

        protected override bool CanExecute(DropInfo parameter)
        {
            if (parameter == null || !CanExecute())
            {
                return false;
            }

            var data = parameter.Data as IDataObject;

            if (data == null)
            {
                return false;
            }

            return data.GetDataPresent(DataFormats.FileDrop);
        }

        protected virtual bool CanExecute()
        {
            return true;
        }

        protected override void Execute(DropInfo parameter)
        {
            var data = (IDataObject)parameter.Data;

            var filenames = (string[])data.GetData(DataFormats.FileDrop);

            if (filenames == null)
            {
                return;
            }

            IEnumerable<Track> tracks;
            if (!progressDialog.TryExecute(
                (token, progress) =>
                    {
                        var task = library.ImportAsync(filenames, token, progress);
                        task.Wait();
                        return task.Result;
                    },
                ProgressDialogOptions,
                out tracks))
            {
                return;
            }

            if (!tracks.Any())
            {
                return;
            }

            OnImported(tracks, parameter);
        }

        protected virtual void OnImported(IEnumerable<Track> tracks, IDropInfo dropInfo)
        {
        }
    }
}