using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel
{
    public interface IRelaxedTransitionDetector
    {
        Transition GetTransitionBetween(PlaybackSpeed first, PlaybackSpeed second);
    }

    /// <summary>
    /// Will detect a transition between any two tracks, regardless if they are
    /// compatible or not.
    /// </summary>
    public class RelaxedTransitionDetector : IRelaxedTransitionDetector
    {
        readonly IEnumerable<IMixingStrategy> strategies;

        public RelaxedTransitionDetector(
            IEnumerable<IMixingStrategy> strategies)
        {
            if (strategies == null) throw new ArgumentNullException("strategies");
            this.strategies = strategies;
        }

        public Transition GetTransitionBetween(PlaybackSpeed first, PlaybackSpeed second)
        {
            if (first == null && second == null)
                throw new ArgumentException("Cannot detect transition between two null tracks.");

            if (first == null)
                return Transition.Intro(second.ActualKey);

            if (second == null)
                return Transition.Outro(first.ActualKey);

            var strategy = strategies.First(s => s.IsCompatible(first, second));

            return new Transition(first.ActualKey, second.ActualKey, strategy);
        }
    }
}