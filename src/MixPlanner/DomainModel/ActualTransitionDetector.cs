using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel
{
    public interface IActualTransitionDetector
    {
        Transition GetTransitionBetween(PlaybackSpeed first, PlaybackSpeed second);
    }

    /// <summary>
    /// Will detect the transition between any two tracks, regardless if they are
    /// compatible or not.
    /// </summary>
    public class ActualTransitionDetector : IActualTransitionDetector
    {
        readonly IEnumerable<IMixingStrategy> strategies;

        public ActualTransitionDetector(
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