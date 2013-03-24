using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.AutoMixing;
using MixPlanner.Events;

namespace MixPlanner.Commands
{
    public class AutoMixCommand : AsyncCommandBase<IEnumerable<IMixItem>>
    {
        readonly IMix mix;
        readonly IAutoMixingContextFactory contextFactory;
        readonly IDispatcherMessenger messenger;
        readonly IAutoMixingStrategy strategy;

        public AutoMixCommand(
            IMix mix,
            IAutoMixingStrategy strategy,
            IAutoMixingContextFactory contextFactory,
            IDispatcherMessenger messenger)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            if (strategy == null) throw new ArgumentNullException("strategy");
            if (contextFactory == null) throw new ArgumentNullException("contextFactory");
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.mix = mix;
            this.strategy = strategy;
            this.contextFactory = contextFactory;
            this.messenger = messenger;
        }

        protected override bool CanExecute(IEnumerable<IMixItem> parameter)
        {
            return !mix.IsLocked && parameter != null && parameter.Any();

            // todo: only contiguous blocks?
        }

        protected override async Task DoExecute(IEnumerable<IMixItem> parameter)
        {
            await Task.Factory.StartNew(() => DoExecuteSync(parameter));
        }

        void DoExecuteSync(IEnumerable<IMixItem> parameter)
        {
            using (mix.Lock())
            {
                messenger.SendToUI(new BeganAutoMixingEvent());

                mix.AutoAdjustBpms(parameter);
                AutoMixingContext context = contextFactory.CreateContext(mix, parameter);
                AutoMixingResult results = strategy.AutoMix(context);

                if (!results.IsSuccess)
                    return;

                int startIndex = mix.IndexOf(parameter.First());

                for (int i = 0; i < results.MixedTracks.Count; i++)
                {
                    IMixItem item = results.MixedTracks[i];
                    mix.Reorder(item, startIndex + i);
                }

                foreach (IMixItem unknownTrack in results.UnknownTracks)
                    mix.MoveToEnd(unknownTrack);
            }
        }
    }
}