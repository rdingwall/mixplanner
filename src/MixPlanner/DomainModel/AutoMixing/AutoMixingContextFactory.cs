using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel.AutoMixing
{
    public interface IAutoMixingContextFactory
    {
        AutoMixingContext CreateContext(IMix mix, IEnumerable<IMixItem> selectedTracks);
    }

    public class AutoMixingContextFactory : IAutoMixingContextFactory
    {
        public AutoMixingContext CreateContext(IMix mix, IEnumerable<IMixItem> selectedTracks)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            if (selectedTracks == null) throw new ArgumentNullException("selectedTracks");

            if (selectedTracks.Count() == mix.Count)
            {
                // Entire mix is selected, everything can be moved (no
                // preceeeding/following keys that need to be observed)
                return new AutoMixingContext(selectedTracks);
            }

            IMixItem preceeding = mix.GetPreceedingItem(selectedTracks.First());
            IMixItem following = mix.GetFollowingItem(selectedTracks.Last());

            // Only uphold preceeding/following keys if they are known.
            IgnoreIfUnknownKeyOrBpm(ref preceeding);
            IgnoreIfUnknownKeyOrBpm(ref following);

            return new AutoMixingContext(selectedTracks, preceeding, following);
        }

        void IgnoreIfUnknownKeyOrBpm(ref IMixItem track)
        {
            if (track != null && track.IsUnknownKeyOrBpm)
                track = null;
        }
    }
}