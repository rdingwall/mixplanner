using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class TrackRemovedFromMixEvent
    {
        public IMixItem Item { get; private set; }

        public TrackRemovedFromMixEvent(IMixItem item)
        {
            if (item == null) throw new ArgumentNullException("item");
            Item = item;
        }
    }
}