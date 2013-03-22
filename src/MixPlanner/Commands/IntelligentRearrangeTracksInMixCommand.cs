using System;
using System.Collections.Generic;
using System.Linq;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    public class IntelligentRearrangeTracksInMixCommand : CommandBase<IEnumerable<MixItem>>
    {
        readonly IMix mix;

        public IntelligentRearrangeTracksInMixCommand(IMix mix)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            this.mix = mix;
        }

        protected override bool CanExecute(IEnumerable<MixItem> parameter)
        {
            return parameter != null && parameter.Any();
        }

        protected override void Execute(IEnumerable<MixItem> parameter)
        {
            
        }
    }
}