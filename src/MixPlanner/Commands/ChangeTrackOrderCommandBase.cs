﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MixPlanner.DomainModel;

namespace MixPlanner.Commands
{
    public abstract class ChangeTrackOrderCommandBase : AsyncCommandBase<IEnumerable<IMixItem>>
    {
        protected readonly IMix Mix;
        protected readonly IDispatcherMessenger Messenger;

        protected ChangeTrackOrderCommandBase(
            IMix mix,
            IDispatcherMessenger messenger)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.Mix = mix;
            this.Messenger = messenger;
        }

        protected override bool CanExecute(IEnumerable<IMixItem> parameter)
        {
            return !Mix.IsLocked && parameter != null && parameter.Any();

            // todo: only contiguous blocks?
        }

        protected override async Task DoExecute(IEnumerable<IMixItem> parameter)
        {
            await Task.Factory.StartNew(() => DoExecuteSync(parameter));
        }

        protected abstract bool TryCalculateNewOrderOfTracks(
            IEnumerable<IMixItem> selectedItems, out IEnumerable<IMixItem> newOrder);

        void DoExecuteSync(IEnumerable<IMixItem> parameter)
        {
            using (Mix.Lock())
            using (new DisableRecommendationsScope(Messenger))
            {
                IEnumerable<IMixItem> newOrder;
                if (!TryCalculateNewOrderOfTracks(parameter, out newOrder))
                    return;

                ApplyNewOrdering(newOrder, originalOrder: parameter);
            }
        }

        void ApplyNewOrdering(IEnumerable<IMixItem> newOrder, IEnumerable<IMixItem> originalOrder)
        {
            // Re-ordered results will be moved to a contiguous block starting
            // at the index of the first track in the original un-mixed
            // selection.
            var newOrderList = newOrder.ToList();
            int startIndex = Mix.IndexOf(originalOrder.First());

            for (int i = 0; i < newOrderList.Count; i++)
            {
                IMixItem item = newOrderList[i];
                Mix.Reorder(item, startIndex + i);
            }
        } 
    }
}