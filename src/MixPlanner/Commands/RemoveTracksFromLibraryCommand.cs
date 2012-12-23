using System;
using System.Collections.Generic;
using System.Linq;
using MixPlanner.DomainModel;
using MixPlanner.ViewModels;

namespace MixPlanner.Commands
{
    public class RemoveTracksFromLibraryCommand : CommandBase<IEnumerable<TrackLibraryItemViewModel>>
    {
        readonly ITrackLibrary library;

        public RemoveTracksFromLibraryCommand(ITrackLibrary library)
        {
            if (library == null) throw new ArgumentNullException("library");
            this.library = library;
        }

        protected override bool CanExecute(IEnumerable<TrackLibraryItemViewModel> parameter)
        {
            return parameter != null && parameter.Any();
        }

        protected override void Execute(IEnumerable<TrackLibraryItemViewModel> parameter)
        {
            library.RemoveRange(parameter.Select(i => i.Track));
        }
    }
}