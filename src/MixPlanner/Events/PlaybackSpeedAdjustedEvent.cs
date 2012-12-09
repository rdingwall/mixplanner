using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class PlaybackSpeedAdjustedEvent
    {
        public MixItem MixItem { get; private set; }

        public PlaybackSpeedAdjustedEvent(MixItem item)
        {
            if (item == null) throw new ArgumentNullException("item");
            MixItem = item;
        }
    }
}