namespace MixPlanner.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GongSolutions.Wpf.DragDrop;
    using MixPlanner.DomainModel;
    using MixPlanner.ViewModels;

    public sealed class AddTrackToMixCommand : CommandBase<DropInfo>
    {
        private readonly ICurrentMixProvider mixProvider;

        public AddTrackToMixCommand(ICurrentMixProvider mixProvider)
        {
            if (mixProvider == null)
            {
                throw new ArgumentNullException("mixProvider");
            }

            this.mixProvider = mixProvider;
        }

        protected override bool CanExecute(DropInfo parameter)
        {
            if (parameter == null)
            {
                return false;
            }

            IMix mix = mixProvider.GetCurrentMix();

            if (mix.IsLocked)
            {
                return false;
            }

            return parameter.Data is TrackLibraryItemViewModel || IsCollection(parameter.Data);
        }

        protected override void Execute(DropInfo parameter)
        {
            var sourceItem = parameter.Data as TrackLibraryItemViewModel;

            IMix mix = mixProvider.GetCurrentMix();

            if (sourceItem != null)
            {
                mix.Insert(sourceItem.Track, parameter.InsertIndex);
                return;
            }

            var sourceItems = parameter.Data as IEnumerable<TrackLibraryItemViewModel>;

            if (sourceItems != null && sourceItems.Any())
            {
                mix.Insert(sourceItems.Select(i => i.Track), parameter.InsertIndex);
            }
        }

        private static bool IsCollection(object obj)
        {
            var items = obj as IEnumerable<TrackLibraryItemViewModel>;
            return items != null && items.Any();
        }
    }
}