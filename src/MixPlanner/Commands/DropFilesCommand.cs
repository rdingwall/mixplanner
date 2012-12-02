using System;
using System.Windows;
using System.Windows.Input;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    public class DropFilesCommand : ICommand
    {
        readonly ITrackLibrary library;

        public DropFilesCommand(ITrackLibrary library)
        {
            if (library == null) throw new ArgumentNullException("library");
            this.library = library;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter == null) return;

            var e = (DragEventArgs)parameter;

            var files = (string[]) e.Data.GetData(DataFormats.FileDrop);

            if (files == null) return;

            library.Import(files);
        }

        public event EventHandler CanExecuteChanged;
    }
}