using System;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.DomainModel;
using MixPlanner.ViewModels;

namespace MixPlanner.Commands
{
    public class DropTrackIntoMixCommand : CommandBase<DropInfo>
    {
        readonly IMix mix;

        public DropTrackIntoMixCommand(IMix mix)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            this.mix = mix;
        }

        protected override bool CanExecute(DropInfo parameter)
        {
            return parameter != null;
        }

        protected override void Execute(DropInfo parameter)
        {
            var sourceItem = parameter.Data as LibraryItemViewModel;

            mix.Insert(sourceItem.Track, parameter.InsertIndex);
        }
    }
}