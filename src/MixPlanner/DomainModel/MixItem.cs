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
            PlaybackSpeed = track.GetDefaultPlaybackSpeed();
        }

        public PlaybackSpeed PlaybackSpeed { get; private set; }
        public Track Track { get; private set; }
        public Transition Transition { get; set; }

        public void SetPlaybackSpeed(double value)
        {
            PlaybackSpeed.SetSpeed(value);
            
        }
    }
}