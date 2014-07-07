namespace MixPlanner.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Design.PluralizationServices;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using GalaSoft.MvvmLight.Messaging;
    using MixPlanner.DomainModel;

    public sealed class RemoveTracksFromLibraryCommand : CommandBase<IEnumerable<Track>>
    {
        private readonly IDispatcherMessenger messenger;
        private readonly ITrackLibrary library;
        private readonly PluralizationService pluralizer;

        public RemoveTracksFromLibraryCommand(
            IDispatcherMessenger messenger, 
            ITrackLibrary library)
        {
            if (messenger == null)
            {
                throw new ArgumentNullException("messenger");
            }

            if (library == null)
            {
                throw new ArgumentNullException("library");
            }

            this.messenger = messenger;
            this.library = library;
            pluralizer = PluralizationService.CreateService(CultureInfo.CurrentCulture);
        }

        protected override bool CanExecute(IEnumerable<Track> parameter)
        {
            return parameter != null && parameter.Any();
        }

        protected override void Execute(IEnumerable<Track> parameter)
        {
            var trackOrTracks = pluralizer.Pluralize(parameter, "track");
            var content =
                string.Format(
                    "Are you sure you want to remove the selected {0} from the library?{1}{1}(The underlying files will not be deleted.)",
                    trackOrTracks,
                    Environment.NewLine);

            var message =
                new DialogMessage(this, content, m => OnRemoveConfirmed(m, parameter))
                    {
                        Button = MessageBoxButton.OKCancel,
                        Caption = "MixPlanner",
                    };

            messenger.SendToUI(message);
        }

        private async void OnRemoveConfirmed(MessageBoxResult obj, IEnumerable<Track> parameter)
        {
            if (obj != MessageBoxResult.OK)
            {
                return;
            }

            await library.RemoveRangeAsync(parameter);
        }
    }
}