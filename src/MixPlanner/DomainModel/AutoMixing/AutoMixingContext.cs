using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel.AutoMixing
{
    public class AutoMixingContext
    {
        public AutoMixingContext(
            IEnumerable<IMixItem> tracksToMix,
            IMixItem preceedingTrack = null,
            IMixItem followingTrack = null)
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

        public IMixItem PreceedingTrack { get; private set; }
        public IEnumerable<IMixItem> TracksToMix { get; private set; }
        public IMixItem FollowingTrack { get; private set; }
        public int TracksToMixCount { get { return TracksToMix.Count(); } }

        public HarmonicKey GetOptionalStartKey()
        {
            return PreceedingTrack != null ? PreceedingTrack.ActualKey : null;
        }

        public HarmonicKey GetOptionalEndKey()
        {
            return FollowingTrack != null ? FollowingTrack.ActualKey : null;
        }
    }
}