using System;
using System.Collections.Generic;
using System.Linq;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.DomainModel;
using MixPlanner.ViewModels;

namespace MixPlanner.Commands
{
    public class AddTrackToMixCommand : CommandBase<DropInfo>
    {
        readonly IMix mix;

        public AddTrackToMixCommand(IMix mix)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            this.mix = mix;
        }

        protected override bool CanExecute(DropInfo parameter)
        {
            if (parameter == null)
                return false;

            return parameter.Data is TrackLibraryItemViewModel || IsCollection(parameter.Data);
        }

        static bool IsCollection(object obj)
        {
            var items = obj as IEnumerable<TrackLibraryItemViewModel>;
            return items != null && items.Any();
        }

        protected override void Execute(DropInfo parameter)
        {
            var sourceItem = parameter.Data as TrackLibraryItemViewModel;

            if (sourceItem != null)
            {
                mix.Insert(sourceItem.Track, parameter.InsertIndex);
                return;
            }

            var sourceItems = parameter.Data as IEnumerable<TrackLibraryItemViewModel>;

            if (sourceItems != null && sourceItems.Any())
                mix.Insert(sourceItems.Select(i => i.Track), parameter.InsertIndex);
        }
    }
}