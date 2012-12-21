using System;
using System.Collections.Generic;
using System.Linq;
using MixPlanner.DomainModel;
using MixPlanner.ViewModels;

namespace MixPlanner.Commands
{
    public class RemoveTrackFromLibraryCommand : CommandBase<IEnumerable<LibraryItemViewModel>>
    {
        readonly ITrackLibrary library;

        public RemoveTrackFromLibraryCommand(ITrackLibrary library)
        {
            if (library == null) throw new ArgumentNullException("library");
            this.library = library;
        }

        protected override bool CanExecute(IEnumerable<LibraryItemViewModel> parameter)
        {
            return parameter != null && parameter.Any();
        }

        protected override void Execute(IEnumerable<LibraryItemViewModel> parameter)
        {
            library.RemoveRange(parameter.Select(i => i.Track));
        }
    }
}