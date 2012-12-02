using System;
using System.Windows.Input;
using MixPlanner.DomainModel;
using MixPlanner.ViewModels;

namespace MixPlanner.Commands
{
    public class RemoveTrackFromLibraryCommand : ICommand
    {
        readonly ITrackLibrary library;

        public RemoveTrackFromLibraryCommand(ITrackLibrary library)
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

            var item = (LibraryItemViewModel) parameter;

            library.Remove(item.Track);
        }

        public event EventHandler CanExecuteChanged;
    }
}