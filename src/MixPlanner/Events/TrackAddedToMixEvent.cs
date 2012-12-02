using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class TrackAddedToMixEvent
    {
        public Track Track { get; private set; }
        public int InsertIndex { get; private set; }

        public TrackAddedToMixEvent(Track track, int insertIndex)
        {
            if (track == null) throw new ArgumentNullException("track");
            Track = track;
            InsertIndex = insertIndex;
        }
    }
}