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
            var e = parameter as DragEventArgs;

            var files = (string[]) e.Data.GetData(DataFormats.FileDrop);

            library.Import(files);
        }

        public event EventHandler CanExecuteChanged;
    }
}