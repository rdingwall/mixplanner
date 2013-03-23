using System;
using System.Collections.Generic;
using System.Linq;
using MixPlanner.DomainModel;
using MoreLinq;

namespace MixPlanner.Commands
{
    public class ResetPlaybackSpeedCommand : CommandBase<IEnumerable<IMixItem>>
    {
        readonly IMix mix;

        public ResetPlaybackSpeedCommand(IMix mix)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            this.mix = mix;
        }

        protected override bool CanExecute(IEnumerable<IMixItem> parameter)
        {
            return !mix.IsLocked && parameter != null && parameter.Any();
        }

        protected override void Execute(IEnumerable<IMixItem> parameter)
        {
            parameter.ForEach(m => mix.ResetPlaybackSpeed(m));
        }
    }
}