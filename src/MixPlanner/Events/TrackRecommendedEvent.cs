using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class TrackRecommendedEvent
    {
        public TrackRecommendedEvent(Track track, double recommendationFactor)
        {
            if (track == null) throw new ArgumentNullException("track");
            Track = track;
            RecommendationFactor = recommendationFactor;
        }

        public Track Track { get; private set; }
        public double RecommendationFactor { get; private set; }
    }
}