using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class TrackRecommendedEvent
    {
        public TrackRecommendedEvent(Track track, double compatibilityFactor)
        {
            if (track == null) throw new ArgumentNullException("track");
            Track = track;
            CompatibilityFactor = compatibilityFactor;
        }

        public Track Track { get; private set; }
        public double CompatibilityFactor { get; private set; }
    }
}