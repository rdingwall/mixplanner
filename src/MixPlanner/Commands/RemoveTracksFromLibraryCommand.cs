using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    public class RemoveTracksFromLibraryCommand : AsyncCommandBase<IEnumerable<Track>>
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

        protected override async Task DoExecute(IEnumerable<Track> parameter)
        {
            await library.RemoveRangeAsync(parameter);
        }
    }
}