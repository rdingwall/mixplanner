using System;
using MixPlanner.DomainModel;
using MixPlanner.ViewModels;

namespace MixPlanner.Commands
{
    public class RemoveTrackFromLibraryCommand : CommandBase<LibraryItemViewModel>
    {
        readonly ITrackLibrary library;

        public RemoveTrackFromLibraryCommand(ITrackLibrary library)
        {
            if (library == null) throw new ArgumentNullException("library");
            this.library = library;
        }

        protected override bool CanExecute(LibraryItemViewModel parameter)
        {
            return parameter != null;
        }

        protected override void Execute(LibraryItemViewModel parameter)
        {
            library.Remove(parameter.Track);
        }
    }
}