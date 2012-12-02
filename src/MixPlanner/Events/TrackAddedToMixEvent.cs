using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class TrackAddedToMixEvent
    {
        public TrackAddedToMixEvent(MixItem item, int insertIndex)
        {
            if (item == null) throw new ArgumentNullException("item");
            Item = item;
            InsertIndex = insertIndex;
        }

        public MixItem Item { get; private set; }
        public int InsertIndex { get; private set; }
    }
}