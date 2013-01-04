using System;
using System.Collections.Generic;
using System.Linq;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    public class RemoveTracksFromLibraryCommand : CommandBase<IEnumerable<Track>>
    {
        readonly ITrackLibrary library;

        public RemoveTracksFromLibraryCommand(ITrackLibrary library)
        {
            if (library == null) throw new ArgumentNullException("library");
            this.library = library;
        }

        protected override bool CanExecute(IEnumerable<Track> parameter)
        {
            return parameter != null && parameter.Any();
        }

        protected override void Execute(IEnumerable<Track> parameter)
        {
            library.RemoveRange(parameter);
        }
    }
}