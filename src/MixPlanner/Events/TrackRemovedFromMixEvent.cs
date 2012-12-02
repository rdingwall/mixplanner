using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class TrackRemovedFromMixEvent
    {
        public MixItem Item { get; private set; }

        public TrackRemovedFromMixEvent(MixItem item)
        {
            if (item == null) throw new ArgumentNullException("item");
            Item = item;
        }
    }
}