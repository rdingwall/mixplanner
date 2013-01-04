using System;

namespace MixPlanner.DomainModel
{
    public class MixItem
    {
        public MixItem(Mix mix, Track track, Transition transition)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            if (track == null) throw new ArgumentNullException("track");
            Mix = mix;
            Track = track;
            Transition = transition;
            PlaybackSpeed = track.GetDefaultPlaybackSpeed();
        }

        public PlaybackSpeed PlaybackSpeed { get; private set; }
        public Mix Mix { get; private set; }
        public Track Track { get; private set; }
        public Transition Transition { get; set; }

        public void SetPlaybackSpeed(double value)
        {
            PlaybackSpeed.SetSpeed(value);
        }

        public void ResetPlaybackSpeed()
        {
            PlaybackSpeed.Reset();
        }
    }
}