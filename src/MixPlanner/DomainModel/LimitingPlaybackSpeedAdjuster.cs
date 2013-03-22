using System;

namespace MixPlanner.DomainModel
{
    public interface ILimitingPlaybackSpeedAdjuster : IPlaybackSpeedAdjuster { }

    /// <summary>
    /// Will suggest adjustments, but only up to +/- 6%.
    /// </summary>
    public class LimitingPlaybackSpeedAdjuster : 
        PlaybackSpeedAdjuster, ILimitingPlaybackSpeedAdjuster
    {
        const double MaxPermittedDifference = 0.06001;

        public override double GetSuggestedIncrease(PlaybackSpeed track, double targetBpm)
        {
            double increase = base.GetSuggestedIncrease(track, targetBpm);

            if (Math.Abs(increase) > MaxPermittedDifference)
                return 0;

            return increase;
        }
    }
}