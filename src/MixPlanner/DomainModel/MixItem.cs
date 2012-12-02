using System;

namespace MixPlanner.DomainModel
{
    public class MixItem
    {
        public MixItem(Track track, Transition previousTransition, Transition nextTransition)
        {
            if (track == null) throw new ArgumentNullException("track");
            Track = track;
            PreviousTransition = previousTransition;
            NextTransition = nextTransition;
        }

        public Track Track { get; private set; }
        public Transition PreviousTransition { get; set; }
        public Transition NextTransition { get; set; }
    }
}