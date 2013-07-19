using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class PlayerPlayingEvent : TrackEvent
    {
        public PlayerPlayingEvent(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            Track = track;
        }
    }
}