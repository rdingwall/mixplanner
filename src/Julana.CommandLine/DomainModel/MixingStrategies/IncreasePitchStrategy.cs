using System;
using System.Collections.Generic;
using System.Linq;

namespace Julana.CommandLine.DomainModel.MixingStrategies
{
    public abstract class IncreasePitchStrategy : IMixingStrategy
    {
        readonly int increaseAmount;

        protected IncreasePitchStrategy(int increaseAmount)
        {
            this.increaseAmount = increaseAmount;
        }

        public IEnumerable<Track> NextSuggestedTracks(Track currentTrack, IEnumerable<Track> unplayedTracks)
        {
            if (currentTrack == null) throw new ArgumentNullException("currentTrack");
            if (unplayedTracks == null) throw new ArgumentNullException("unplayedTracks");

            return unplayedTracks
                .Where(t => t.Key.HasSameScaleAs(currentTrack.Key))
                .Where(t => t.Key.Equals(currentTrack.Key.IncreasePitch(increaseAmount)));
        }
    }
}