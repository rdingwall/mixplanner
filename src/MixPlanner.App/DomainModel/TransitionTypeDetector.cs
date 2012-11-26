using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.App.DomainModel
{
    public interface ITransitionTypeDetector
    {
        Transition GetTransitionBetween(Track firstTrack, Track secondTrack);
    }

    public class TransitionTypeDetector : ITransitionTypeDetector
    {
        readonly IEnumerable<IMixingStrategy> strategies;

        public TransitionTypeDetector(IEnumerable<IMixingStrategy> strategies)
        {
            if (strategies == null) throw new ArgumentNullException("strategies");
            this.strategies = strategies;
        }

        public Transition GetTransitionBetween(Track firstTrack, Track secondTrack)
        {
            if (firstTrack == null) throw new ArgumentNullException("firstTrack");
            if (secondTrack == null) throw new ArgumentNullException("secondTrack");

            var strategy = strategies.First(s => s.IsCompatible(firstTrack, secondTrack));

            return new Transition(firstTrack.Key, strategy, secondTrack.Key);
        }
    }
}