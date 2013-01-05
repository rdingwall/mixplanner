using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel
{
    public interface IStrictTransitionDetector
    {
        Transition GetTransitionBetween(PlaybackSpeed first, PlaybackSpeed second);
    }

    /// <summary>
    /// Will only detect a transition between two tracks if they can be mixed
    /// using a preferred strategy (with compatible keys and BPMs).
    /// </summary>
    public class StrictTransitionDetector : IStrictTransitionDetector
    {
        readonly IEnumerable<IMixingStrategy> preferredStrategies;

        public StrictTransitionDetector(
            IEnumerable<IMixingStrategy> preferredStrategies)
        {
            if (preferredStrategies == null) throw new ArgumentNullException("preferredStrategies");
            this.preferredStrategies = preferredStrategies;
        }

        public Transition GetTransitionBetween(PlaybackSpeed first, PlaybackSpeed second)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            var strategy = preferredStrategies.FirstOrDefault(s => s.IsCompatible(first, second));
            if (strategy == null)
                return null;

            return new Transition
                       {
                           FromKey = first.ActualKey,
                           ToKey = second.ActualKey,
                           Strategy = strategy,
                           Description = ">>> " + strategy.Description
                       };
        }
    }
}