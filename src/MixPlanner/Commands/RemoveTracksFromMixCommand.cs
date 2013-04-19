using System;
using System.Collections.Generic;
using System.Linq;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    public class RemoveTracksFromMixCommand : CommandBase<IEnumerable<IMixItem>>
    {
        readonly IMix mix;
        readonly IDispatcherMessenger messenger;

        public RemoveTracksFromMixCommand(IMix mix, IDispatcherMessenger messenger)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.mix = mix;
            this.messenger = messenger;
        }

        protected override bool CanExecute(IEnumerable<IMixItem> parameter)
        {
            return !mix.IsLocked && parameter != null && parameter.Any();
        }

        protected override void Execute(IEnumerable<IMixItem> parameter)
        {
            using (mix.Lock())
            {
                // Optimization: if we are nuking the whole mix (all tracks
                // selected), it is quicker to clear than remove individual tracks.
                if (parameter.Count() == mix.Count)
                {
                    mix.Clear();
                    return;
                }

                // Have to disable recommendations otherwise it recalcs
                // recommendations every time you delete a row.
                using (new DisableRecommendationsScope(messenger))
                    mix.RemoveRange(parameter);
            }
        }
    }
}