using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Threading;
using MixPlanner.DomainModel;
using MixPlanner.Mp3;
using MixPlanner.ViewModels;

namespace MixPlanner.Commands
{
    public class ReloadTrackFileCommand : AsyncCommandBase<EditTrackWindowViewModel>
    {
        readonly ITrackLoader loader;

        public ReloadTrackFileCommand(ITrackLoader loader)
        {
            if (loader == null) throw new ArgumentNullException("loader");
            this.loader = loader;
        }

        protected override async Task DoExecute(EditTrackWindowViewModel parameter)
        {
            if (String.IsNullOrEmpty(parameter.FilePath))
                return;

            var track = await loader.LoadAsync(parameter.FilePath);

            DispatcherHelper.InvokeAsync(() => UpdateFields(parameter, track));
        }

        void UpdateFields(EditTrackWindowViewModel parameter, Track track)
        {
            parameter.Artist = track.Artist;
            parameter.Title = track.Title;
            parameter.Genre = track.Genre;
            parameter.Year = track.Year;
            parameter.HarmonicKey = track.OriginalKey;
            parameter.Bpm = track.OriginalBpm;
            parameter.Label = track.Label;
        }
    }
}