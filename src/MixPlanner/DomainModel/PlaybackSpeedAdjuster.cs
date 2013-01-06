using System;

namespace MixPlanner.DomainModel
{
    public interface IPlaybackSpeedAdjuster
    {
        double GetSuggestedIncrease(PlaybackSpeed first, PlaybackSpeed second);
    }

    public class PlaybackSpeedAdjuster : IPlaybackSpeedAdjuster
    {
        const double MaxPermittedDifference = 0.06001;

        public double GetSuggestedIncrease(PlaybackSpeed first, PlaybackSpeed second)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            if (second.IsWithinBpmRange(first))
                return 0;

            var increaseRequired = second.GetExactIncreaseRequiredToMatch(first);
            var nearestInterval = increaseRequired.FloorToNearest(HarmonicKeyChangeInterval.Value);

            if (Math.Abs(nearestInterval) > MaxPermittedDifference)
                return 0;

            return nearestInterval;
        }
    }
}