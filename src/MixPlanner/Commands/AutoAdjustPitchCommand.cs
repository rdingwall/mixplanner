namespace MixPlanner.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MixPlanner.DomainModel;

    public sealed class AutoAdjustPitchCommand : CommandBase<IEnumerable<IMixItem>>
    {
        private readonly ICurrentMixProvider mixProvider;

        public AutoAdjustPitchCommand(ICurrentMixProvider mixProvider)
        {
            if (mixProvider == null)
            {
                throw new ArgumentNullException("mixProvider");
            }

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