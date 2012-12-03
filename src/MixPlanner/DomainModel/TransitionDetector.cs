using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel
{
    public interface ITransitionDetector
    {
        Transition GetTransitionBetween(Track firstTrack, Track secondTrack);
    }

    public class TransitionDetector : ITransitionDetector
    {
        readonly IEnumerable<IMixingStrategy> strategies;

        public TransitionDetector(IEnumerable<IMixingStrategy> strategies)
        {
            if (strategies == null) throw new ArgumentNullException("strategies");
            this.strategies = strategies;
        }

        public Transition GetTransitionBetween(Track firstTrack, Track secondTrack)
        {
            var transition = new Transition();

            if (firstTrack != null)
                transition.FromKey = firstTrack.OriginalKey;

            if (secondTrack != null)
                transition.ToKey = secondTrack.OriginalKey;

            if (firstTrack != null && secondTrack != null)
            {
                var strategy = strategies.First(s => s.IsCompatible(firstTrack, secondTrack));

                transition.Strategy = strategy;
                transition.Description = ">>> " + strategy.Description;
            }
            else if (firstTrack == null)
                transition.Description = ">>> Intro";
            else
                transition.Description = ">>> Outro";

            return transition;
        }
    }
}