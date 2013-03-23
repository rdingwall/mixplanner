using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class TrackAddedToMixEvent
    {
        public TrackAddedToMixEvent(IMixItem item, int insertIndex)
        {
            if (item == null) throw new ArgumentNullException("item");
            Item = item;
            InsertIndex = insertIndex;
        }

        public IMixItem Item { get; private set; }
        public int InsertIndex { get; private set; }
    }
}