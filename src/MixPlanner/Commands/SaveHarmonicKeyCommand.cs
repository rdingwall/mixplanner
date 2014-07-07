namespace MixPlanner.Commands
{
    using System;
    using System.Threading.Tasks;
    using MixPlanner.DomainModel;
    using MixPlanner.ViewModels;

    public sealed class SaveHarmonicKeyCommand : AsyncCommandBase<QuickEditHarmonicKeyViewModel>
    {
        private readonly ITrackLibrary library;

        public SaveHarmonicKeyCommand(ITrackLibrary library)
        {
            if (library == null)
            {
                throw new ArgumentNullException("library");
            }

            this.library = library;
        }

        protected async override Task DoExecute(QuickEditHarmonicKeyViewModel parameter)
        {
            Track track = parameter.Track;

            track.OriginalKey = parameter.HarmonicKey;

            await library.SaveAsync(track);

            parameter.Close = true;
        }
    }
}