﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.DomainModel;
using MixPlanner.ViewModels;

namespace MixPlanner.Commands
{
    public class RemoveTracksFromLibraryCommand : CommandBase<IEnumerable<Track>>
    {
        readonly IDispatcherMessenger messenger;
        readonly ITrackLibrary library;
        readonly PluralizationService pluralizer;

        public RemoveTracksFromLibraryCommand(
            IDispatcherMessenger messenger, 
            ITrackLibrary library)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (library == null) throw new ArgumentNullException("library");
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
            var content = String.Format("Are you sure you want to remove the selected {0} from the library?", trackOrTracks);

            var message =
                new DialogMessage(this, content, m => OnRemoveConfirmed(m, parameter))
                    {
                        Button = MessageBoxButton.OKCancel,
                        Caption = "MixPlanner",
                    };

            messenger.SendToUI(message);
        }

        async void OnRemoveConfirmed(MessageBoxResult obj, IEnumerable<Track> parameter)
        {
            if (obj != MessageBoxResult.OK)
                return;

            await library.RemoveRangeAsync(parameter);
        }
    }
}