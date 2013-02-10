using System;

namespace MixPlanner.DomainModel
{
    public interface ILimitingPlaybackSpeedAdjuster : IPlaybackSpeedAdjuster { }

    /// <summary>
    /// Will suggest adjustments, but only up to +/- 6%.
    /// </summary>
    public class LimitingPlaybackSpeedAdjuster : ILimitingPlaybackSpeedAdjuster
    {
        const double MaxPermittedDifference = 0.06001;

        readonly IPlaybackSpeedAdjuster adjuster;

        public LimitingPlaybackSpeedAdjuster(IPlaybackSpeedAdjuster adjuster)
        {
            if (adjuster == null) throw new ArgumentNullException("adjuster");
            this.adjuster = adjuster;
        }

        public double GetSuggestedIncrease(PlaybackSpeed first, PlaybackSpeed second)
        {
            var increase = adjuster.GetSuggestedIncrease(first, second);
            if (Math.Abs(increase) > MaxPermittedDifference)
                return 0;

            return increase;
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