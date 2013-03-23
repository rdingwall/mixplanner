using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class TransitionChangedEvent
    {
        public IMixItem MixItem { get; private set; }

        public TransitionChangedEvent(IMixItem mixItem)
        {
            if (mixItem == null) throw new ArgumentNullException("mixItem");
            MixItem = mixItem;
        }
    }
}