using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel
{
    public interface IRecommendedTransitionDetector
    {
        Transition GetTransitionBetween(PlaybackSpeed first, PlaybackSpeed second);
    }

    /// <summary>
    /// Will only detect a transition between two tracks if they can be mixed
    /// using a preferred strategy (with compatible keys and BPMs).
    /// </summary>
    public class RecommendedTransitionDetector : IRecommendedTransitionDetector
    {
        readonly IEnumerable<IMixingStrategy> preferredStrategies;
        readonly ILimitingPlaybackSpeedAdjuster playbackSpeedAdjuster;

        public RecommendedTransitionDetector(
            IEnumerable<IMixingStrategy> preferredStrategies,
            ILimitingPlaybackSpeedAdjuster playbackSpeedAdjuster)
        {
            if (preferredStrategies == null) throw new ArgumentNullException("preferredStrategies");
            if (playbackSpeedAdjuster == null) throw new ArgumentNullException("playbackSpeedAdjuster");
            this.preferredStrategies = preferredStrategies;
            this.playbackSpeedAdjuster = playbackSpeedAdjuster;
        }

        public Transition GetTransitionBetween(PlaybackSpeed first, PlaybackSpeed second)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            // Adjust playback speed to match
            var suggestedSpeedIncrease = playbackSpeedAdjuster.CalculateSuggestedIncrease(first, second);
            var secondAdjusted = second.AsIncreasedBy(suggestedSpeedIncrease);

            var strategy = preferredStrategies.FirstOrDefault(s => s.IsCompatible(first, secondAdjusted));
            if (strategy == null)
                return null;

            return new Transition(
                first.ActualKey, 
                secondAdjusted.ActualKey, 
                strategy, 
                suggestedSpeedIncrease);
        }
    }
}