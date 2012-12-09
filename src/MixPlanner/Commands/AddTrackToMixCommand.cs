using System;
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

        protected override bool DoCanExecute(DropInfo parameter)
        {
            return parameter != null && parameter.Data is LibraryItemViewModel;
        }

        protected override void DoExecute(DropInfo parameter)
        {
            var sourceItem = parameter.Data as LibraryItemViewModel;

            mix.Insert(sourceItem.Track, parameter.InsertIndex);
        }
    }
}