using System;
using System.Collections.Generic;
using System.Linq;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    public class AutoAdjustPitchCommand : CommandBase<IEnumerable<IMixItem>>
    {
        readonly IMix mix;

        public AutoAdjustPitchCommand(IMix mix)
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
            mix.AutoAdjustBpms(parameter);
        }
    }
}