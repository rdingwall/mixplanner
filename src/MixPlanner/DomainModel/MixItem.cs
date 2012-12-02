using System;

namespace MixPlanner.DomainModel
{
    public class MixItem
    {
        public MixItem(Track track, Transition transition)
        {
            if (track == null) throw new ArgumentNullException("track");
            Track = track;
            Transition = transition;
        }

        public Track Track { get; private set; }
        public Transition Transition { get; set; }
    }
}