using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    public class RemoveTrackFromMixCommand : CommandBase<MixItem>
    {
        readonly IMix mix;

        public RemoveTrackFromMixCommand(IMix mix)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            this.mix = mix;
        }

        protected override bool DoCanExecute(MixItem parameter)
        {
            return parameter != null;
        }

        protected override void DoExecute(MixItem parameter)
        {
            mix.Remove(parameter);
        }
    }
}