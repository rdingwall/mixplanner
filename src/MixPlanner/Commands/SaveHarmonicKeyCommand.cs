using System;
using System.Threading.Tasks;
using MixPlanner.DomainModel;
using MixPlanner.ViewModels;

namespace MixPlanner.Commands
{
    public class SaveHarmonicKeyCommand : AsyncCommandBase<QuickEditHarmonicKeyViewModel>
    {
        readonly ITrackLibrary library;

        public SaveHarmonicKeyCommand(ITrackLibrary library)
        {
            if (library == null) throw new ArgumentNullException("library");
            this.library = library;
        }

        protected async override Task DoExecute(QuickEditHarmonicKeyViewModel parameter)
        {
            var track = parameter.Track;

            track.OriginalKey = parameter.HarmonicKey;

            await library.SaveAsync(track);

            parameter.Close = true;
        }
    }
}