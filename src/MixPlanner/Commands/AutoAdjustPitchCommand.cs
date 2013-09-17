using System;
using System.Collections.Generic;
using System.Linq;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    public class AutoAdjustPitchCommand : CommandBase<IEnumerable<IMixItem>>
    {
        readonly ICurrentMixProvider mixProvider;

        public AutoAdjustPitchCommand(ICurrentMixProvider mixProvider)
        {
            if (mixProvider == null) throw new ArgumentNullException("mixProvider");
            this.mixProvider = mixProvider;
        }

        protected override bool CanExecute(IEnumerable<IMixItem> parameter)
        {
            IMix mix = mixProvider.GetCurrentMix();
            return !mix.IsLocked && parameter != null && parameter.Any();
        }

        protected override void Execute(IEnumerable<IMixItem> parameter)
        {
            IMix mix = mixProvider.GetCurrentMix();
            mix.AutoAdjustBpms(parameter);
        }
    }
}