using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel.AutoMixing
{
    public interface IAutoMixingContextFactory
    {
        AutoMixingContext<MixItem> CreateContext(IMix mix, IEnumerable<MixItem> selectedTracks);
    }

    public class AutoMixingContextFactory : IAutoMixingContextFactory
    {
        public AutoMixingContext<MixItem> CreateContext(IMix mix, IEnumerable<MixItem> selectedTracks)
        {
            if (mix == null) throw new ArgumentNullException("mix");
            if (selectedTracks == null) throw new ArgumentNullException("selectedTracks");

            if (selectedTracks.Count() == mix.Count)
                return new AutoMixingContext<MixItem>(selectedTracks);

            var preceeding = mix.GetPreceedingItem(selectedTracks.First());
            var following = mix.GetFollowingItem(selectedTracks.Last());

            return new AutoMixingContext<MixItem>(selectedTracks, preceeding, following);

        }
    }
}