using System;
using GongSolutions.Wpf.DragDrop;
using MixPlanner.DomainModel;
using MixPlanner.ViewModels;

namespace MixPlanner.Commands
{
    public class ReorderMixTrackCommand : CommandBase<DropInfo>
    {
        readonly IMix mix;

        public ReorderMixTrackCommand(IMix mix)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            this.mix = mix;
        }

        protected override bool DoCanExecute(DropInfo parameter)
        {
            return parameter != null;
        }

        protected override void DoExecute(DropInfo parameter)
        {
            var sourceItem = parameter.Data as MixItemViewModel;

            mix.Reorder(sourceItem.MixItem, parameter.InsertIndex);
        }
    }
}