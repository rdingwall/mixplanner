using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class TrackRecommendedEvent
    {
        public TrackRecommendedEvent(Track track, Transition transition)
        {
            if (track == null) throw new ArgumentNullException("track");
            if (transition == null) throw new ArgumentNullException("transition");
            Track = track;
            Transition = transition;
        }

        public Track Track { get; private set; }
        public Transition Transition { get; private set; }
    }
}