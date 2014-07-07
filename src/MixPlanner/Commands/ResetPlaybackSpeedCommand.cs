namespace MixPlanner.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MixPlanner.DomainModel;
    using MoreLinq;

    public sealed class ResetPlaybackSpeedCommand : CommandBase<IEnumerable<IMixItem>>
    {
        private readonly ICurrentMixProvider mixProvider;

        public ResetPlaybackSpeedCommand(ICurrentMixProvider mixProvider)
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
            parameter.ForEach(mix.ResetPlaybackSpeed);
        }
    }
}