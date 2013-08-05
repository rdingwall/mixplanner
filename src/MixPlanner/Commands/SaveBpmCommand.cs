using System;
using System.Threading.Tasks;
using MixPlanner.DomainModel;
using MixPlanner.ViewModels;

namespace MixPlanner.Commands
{
    public class SaveBpmCommand : AsyncCommandBase<QuickEditBpmViewModel>
    {
        ITrackLibrary library;

        public SaveBpmCommand(ITrackLibrary library)
        {
            if (library == null) throw new ArgumentNullException("library");
            this.library = library;
        }

        protected async override Task DoExecute(QuickEditBpmViewModel parameter)
        {
            parameter.Validate();

            if (parameter.HasErrors)
                return;

            var track = parameter.Track;

            track.OriginalBpm = Double.Parse(parameter.Bpm);

            await library.SaveAsync(track);

            parameter.Close = true;
        }
    }
}