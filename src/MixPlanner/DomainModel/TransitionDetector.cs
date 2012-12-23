using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel
{
    public interface ITransitionDetector
    {
        Transition GetTransitionBetween(PlaybackSpeed first, PlaybackSpeed second);
    }

    public class TransitionDetector : ITransitionDetector
    {
        readonly IEnumerable<IMixingStrategy> strategies;

        public TransitionDetector(IEnumerable<IMixingStrategy> strategies)
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
                transition.Description = ">>> " + strategy.Description;
            }
            else if (first == null)
                transition.Description = ">>> Intro";
            else
                transition.Description = ">>> Outro";

            return transition;
        }
    }
}