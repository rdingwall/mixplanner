namespace MixPlanner.Commands
{
    using System;
    using System.Threading.Tasks;
    using GalaSoft.MvvmLight.Threading;
    using MixPlanner.IO.Loader;
    using MixPlanner.ViewModels;

    public sealed class ReloadTrackFileCommand : AsyncCommandBase<EditTrackWindowViewModel>
    {
        private readonly ITrackLoader loader;

        public ReloadTrackFileCommand(ITrackLoader loader)
        {
            if (loader == null)
            {
                throw new ArgumentNullException("loader");
            }

            this.loader = loader;
        }

        protected override async Task DoExecute(EditTrackWindowViewModel parameter)
        {
            if (string.IsNullOrEmpty(parameter.FilePath))
            {
                return;
            }

            var track = await loader.LoadAsync(parameter.FilePath);

            DispatcherHelper.InvokeAsync(() => parameter.PopulateFrom(track));
        }
    }
}