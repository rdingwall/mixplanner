using System;
using System.Threading.Tasks;
using MixPlanner.DomainModel;
using MixPlanner.ViewModels;

namespace MixPlanner.Commands
{
    public class SaveTrackCommand : AsyncCommandBase<EditTrackWindowViewModel>
    {
        readonly ITrackLibrary library;

        public SaveTrackCommand(ITrackLibrary library)
        {
            if (library == null) throw new ArgumentNullException("library");
            this.library = library;
        }

        protected override async Task DoExecute(EditTrackWindowViewModel parameter)
        {
            var track = parameter.Track;
            track.OriginalKey = parameter.HarmonicKey;
            track.OriginalBpm = parameter.Bpm;
            track.Artist = parameter.Artist;
            track.Title = parameter.Title;
            track.Year = parameter.Year;
            track.Genre = parameter.Genre;
            track.Label = parameter.Label;
            track.Filename = parameter.FilePath;

            await library.SaveAsync(track);

            parameter.Close = true;
        }
    }
}