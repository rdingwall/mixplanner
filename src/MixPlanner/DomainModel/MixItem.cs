using System;

namespace MixPlanner.DomainModel
{
    public class MixItem
    {
        public MixItem(
            Mix mix, 
            Track track, 
            Transition transition, 
            PlaybackSpeed playbackSpeed)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            if (track == null) throw new ArgumentNullException("track");
            if (playbackSpeed == null) throw new ArgumentNullException("playbackSpeed");
            Mix = mix;
            Track = track;
            Transition = transition;
            PlaybackSpeed = playbackSpeed;
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
            PlaybackSpeed = Track.GetDefaultPlaybackSpeed();
        }
    }
}