using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel.AutoMixing
{
    public class AutoMixingResult
    {
        public AutoMixingResult(
            IEnumerable<IMixItem> mixedTracks,
            AutoMixingContext context,
            IEnumerable<IMixItem> unknownTracks,
            bool isSuccess)
        {
            if (mixedTracks == null) throw new ArgumentNullException("mixedTracks");
            if (unknownTracks == null) throw new ArgumentNullException("unknownTracks");
            if (context == null) throw new ArgumentNullException("context");
            MixedTracks = mixedTracks.ToList();
            UnknownTracks = unknownTracks;
            IsSuccess = isSuccess;
            Context = context;
        }

        public bool IsSuccess { get; private set; }
        public IList<IMixItem> MixedTracks { get; private set; }
        public IEnumerable<IMixItem> UnknownTracks { get; private set; }
        public AutoMixingContext Context { get; private set; }

        public static AutoMixingResult Success(
            AutoMixingContext context, 
            IEnumerable<IMixItem> mixedTracks,
            IEnumerable<IMixItem> unknownTracks)
        {
            return new AutoMixingResult(
                mixedTracks: mixedTracks,
                context: context,
                unknownTracks: unknownTracks,
                isSuccess: true);
        }

        public static AutoMixingResult Failure(AutoMixingContext context)
        {
            return new AutoMixingResult(
                mixedTracks: context.TracksToMix, 
                context: context, 
                unknownTracks: Enumerable.Empty<MixItem>(),
                isSuccess: false);
        }
    }
}