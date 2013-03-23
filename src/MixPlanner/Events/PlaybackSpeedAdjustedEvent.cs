using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class PlaybackSpeedAdjustedEvent
    {
        public IMixItem MixItem { get; private set; }

        public PlaybackSpeedAdjustedEvent(IMixItem item)
        {
            if (item == null) throw new ArgumentNullException("item");
            MixItem = item;
        }
    }
}