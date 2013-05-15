using System;
using System.Collections.Generic;
using System.Windows;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.DomainModel;
using MixPlanner.ProgressDialog;

namespace MixPlanner.Commands
{
    public class ImportFilesIntoMixCommand : CommandBase<DropInfo>
    {
        readonly ITrackLibrary library;
        readonly IMix mix;
        readonly IProgressDialogService progressDialog;

        static readonly ProgressDialogOptions ProgressDialogOptions
            = new ProgressDialogOptions
                  {
                      Label = "Importing tracks",
                      WindowTitle = "Importing tracks"
                  };

        public ImportFilesIntoMixCommand(ITrackLibrary library, IMix mix,
            IProgressDialogService progressDialog)
        {
            if (library == null) throw new ArgumentNullException("library");
            if (mix == null) throw new ArgumentNullException("mix");
            if (progressDialog == null) throw new ArgumentNullException("progressDialog");
            this.library = library;
            this.mix = mix;
            this.progressDialog = progressDialog;
        }

        protected override bool CanExecute(DropInfo parameter)
        {
            if (parameter == null) return false;
            if (mix.IsLocked) return false;
            var data = (IDataObject)parameter.Data;
            return data.GetDataPresent(DataFormats.FileDrop); 
        }

        protected override void Execute(DropInfo parameter)
        {
            var data = (IDataObject)parameter.Data;

            var filenames = (string[])data.GetData(DataFormats.FileDrop);

            if (filenames == null) return;

            IEnumerable<Track> tracks;
            if (!progressDialog.TryExecute(
                 (token, progress) =>
                     {
                         var task =library.ImportAsync(filenames, token, progress);
                         task.Wait();
                         return task.Result;
                     },
                ProgressDialogOptions, out tracks))
                return;

            mix.Insert(tracks, parameter.InsertIndex);
        }
    }
}