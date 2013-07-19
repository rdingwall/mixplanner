using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class PlayerLoadedEvent
    {
        public PlayerLoadedEvent(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            Track = track;
        }

        public Track Track { get; private set; }
    }
}