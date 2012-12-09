using System;
using System.IO;
using System.Windows;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    public class DropFilesCommand : CommandBase<DragEventArgs>
    {
        readonly ITrackLibrary library;

        public DropFilesCommand(ITrackLibrary library)
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
            var files = (string[]) parameter.Data.GetData(DataFormats.FileDrop);

            if (files == null) return;

            library.Import(files);
        }
    }
}