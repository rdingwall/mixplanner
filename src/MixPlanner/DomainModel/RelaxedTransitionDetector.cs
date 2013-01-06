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
            var transition = new Transition();

            if (first != null)
                transition.FromKey = first.ActualKey;

            if (second != null)
                transition.ToKey = second.ActualKey;

            if (first != null && second != null)
            {
                var strategy = strategies.First(s => s.IsCompatible(first, second));
                transition.Strategy = strategy;
            }

            return transition;
        }
    }
}