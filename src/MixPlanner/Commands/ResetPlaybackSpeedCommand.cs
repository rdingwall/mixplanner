using System.Collections.Generic;
using System.Linq;
using MixPlanner.DomainModel;
using MoreLinq;

namespace MixPlanner.Commands
{
    public class ResetPlaybackSpeedCommand : CommandBase<IEnumerable<MixItem>>
    {
        protected override bool CanExecute(IEnumerable<MixItem> parameter)
        {
            return parameter != null && parameter.Any();
        }

        protected override void Execute(IEnumerable<MixItem> parameter)
        {
            parameter.ForEach(m => m.Mix.ResetPlaybackSpeed(m));
        }
    }
}