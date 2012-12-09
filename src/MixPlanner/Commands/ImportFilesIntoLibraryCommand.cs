﻿using System;
using System.Windows;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    public class ImportFilesIntoLibraryCommand : CommandBase<DragEventArgs>
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

        protected override void Execute(DragEventArgs parameter)
        {
            var filenames = (string[]) parameter.Data.GetData(DataFormats.FileDrop);

            if (filenames == null) return;

            library.Import(filenames);
        }
    }
}