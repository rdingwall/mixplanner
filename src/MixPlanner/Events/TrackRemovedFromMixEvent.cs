using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class TrackRemovedFromMixEvent
    {
        public IMixItem Item { get; private set; }
        public int Index { get; private set; }

        public TrackRemovedFromMixEvent(IMixItem item, int index)
        {
            if (item == null) throw new ArgumentNullException("item");
            Item = item;
            Index = index;
        }
    }
}