using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class TransitionChangedEvent
    {
        public MixItem MixItem { get; private set; }

        public TransitionChangedEvent(MixItem mixItem)
        {
            if (mixItem == null) throw new ArgumentNullException("mixItem");
            MixItem = mixItem;
        }
    }
}