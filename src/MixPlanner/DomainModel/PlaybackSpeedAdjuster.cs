using System;
using System.Collections.Generic;

namespace MixPlanner.DomainModel
{
    public interface IPlaybackSpeedAdjuster
    {
        double GetSuggestedIncrease(PlaybackSpeed track, double targetBpm);

        /// <summary>
        /// Get adjustment required to make second track (proposed) match the
        /// first (currently playing).
        /// </summary>
        double GetSuggestedIncrease(PlaybackSpeed first, PlaybackSpeed second);

        PlaybackSpeed AutoAdjust(PlaybackSpeed first, PlaybackSpeed second);
        PlaybackSpeed AutoAdjust(PlaybackSpeed track, double targetBpm);
        void AutoAdjustAll(IEnumerable<PlaybackSpeed> tracks, double targetBpm);
    }

    public class PlaybackSpeedAdjuster : IPlaybackSpeedAdjuster
    {
        public virtual double GetSuggestedIncrease(PlaybackSpeed track, double targetBpm)
        {
            if (Double.IsNaN(targetBpm) || track.IsUnknownBpm)
                return 0;

            double increaseRequired = track.GetExactIncreaseRequiredToMatch(targetBpm);
            double nearestInterval = increaseRequired.FloorToNearest(PitchFaderStep.Value);

            return nearestInterval;
        }

        public double GetSuggestedIncrease(PlaybackSpeed first, PlaybackSpeed second)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            if (first.IsUnknownBpm || second.IsUnknownBpm)
                return 0;

            return GetSuggestedIncrease(second, first.ActualBpm);
        }

        public PlaybackSpeed AutoAdjust(PlaybackSpeed first, PlaybackSpeed second)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            if (first.IsUnknownBpm || second.IsUnknownBpm)
                return second;

            return AutoAdjust(second, targetBpm: first.ActualBpm);
        }

        public virtual PlaybackSpeed AutoAdjust(PlaybackSpeed track, double targetBpm)
        {
            if (track == null) throw new ArgumentNullException("track");

            if (track.IsUnknownBpm || Double.IsNaN(targetBpm))
                return track;

            double increase = GetSuggestedIncrease(track, targetBpm);

            return track.AsIncreasedBy(increase);
        }

        public virtual void AutoAdjustAll(
            IEnumerable<PlaybackSpeed> tracks, double targetBpm)
        {
            if (tracks == null) throw new ArgumentNullException("tracks");
            if (Double.IsNaN(targetBpm))
                return;

            foreach (var track in tracks)
            {
                double increase = GetSuggestedIncrease(track, targetBpm);
                track.Increase(increase);
            }
        }
    }
}