using System;
using MixPlanner.DomainModel.AutoMixing;

namespace MixPlanner.DomainModel
{
    public class MixItem : IAutoMixable
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
        public HarmonicKey ActualKey { get { return PlaybackSpeed.ActualKey; } }

        public bool IsUnknownKeyOrBpm
        {
            get { return Track.IsUnknownBpm || Track.IsUnknownKey; }
        }

        public void SetPlaybackSpeed(double value)
        {
            PlaybackSpeed.SetSpeed(value);
        }

        public void ResetPlaybackSpeed()
        {
            PlaybackSpeed = Track.GetDefaultPlaybackSpeed();
        }

        public override string ToString()
        {
            return String.Format("{0} {1}", Track, PlaybackSpeed);
        }

        public PlaybackSpeed GetDefaultPlaybackSpeed()
        {
            return Track.GetDefaultPlaybackSpeed();
        }
    }
}