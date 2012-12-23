using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class PlayerLoadedTrackEvent
    {
        public Track Track { get; private set; }

        public PlayerLoadedTrackEvent(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            Track = track;
        }
    }
}