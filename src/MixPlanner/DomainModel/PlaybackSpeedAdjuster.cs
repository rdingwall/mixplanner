using System;

namespace MixPlanner.DomainModel
{
    public interface IPlaybackSpeedAdjuster
    {
        double GetSuggestedIncrease(PlaybackSpeed first, PlaybackSpeed second);
        PlaybackSpeed AutoAdjust(PlaybackSpeed first, PlaybackSpeed second);
    }

    public class PlaybackSpeedAdjuster : IPlaybackSpeedAdjuster
    {
        public double GetSuggestedIncrease(PlaybackSpeed first, PlaybackSpeed second)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            if (first.IsUnknownBpm || second.IsUnknownBpm)
                return 0;

            var increaseRequired = second.GetExactIncreaseRequiredToMatch(first);
            var nearestInterval = increaseRequired.FloorToNearest(PitchFaderStep.Value);

            return nearestInterval;
        }

        public PlaybackSpeed AutoAdjust(PlaybackSpeed first, PlaybackSpeed second)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            var increase = GetSuggestedIncrease(first, second);

            return second.AsIncreasedBy(increase);
        }
    }
}