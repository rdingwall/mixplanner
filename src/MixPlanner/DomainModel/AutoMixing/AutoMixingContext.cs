using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel.AutoMixing
{
    public class AutoMixingContext<T> where T : IAutoMixable
    {
        public AutoMixingContext(
            IEnumerable<T> tracksToMix,
            T preceedingTrack = default(T),
            T followingTrack = default(T))
        {
            if (tracksToMix == null) throw new ArgumentNullException("tracksToMix");

            if (preceedingTrack != null && preceedingTrack.IsUnknownKeyOrBpm)
                throw new ArgumentException("Boundary track cannot have an unknown key or BPM. Please specify null as the starting track instead and try again.", "preceedingTrack");

            if (followingTrack != null && followingTrack.IsUnknownKeyOrBpm)
                throw new ArgumentException("Boundary track cannot have an unknown key or BPM. Please specify null as the starting track instead and try again.", "followingTrack");

            if (!tracksToMix.Any())
                throw new ArgumentException("Must provide at least one track to mix.", "tracksToMix");

            PreceedingTrack = preceedingTrack;
            TracksToMix = tracksToMix;
            FollowingTrack = followingTrack;
        }

        public T PreceedingTrack { get; private set; }
        public IEnumerable<T> TracksToMix { get; private set; }
        public T FollowingTrack { get; private set; }
    }
}