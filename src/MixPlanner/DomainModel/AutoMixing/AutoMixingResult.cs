using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel.AutoMixing
{
    public static class AutoMixingResult
    {
        public static AutoMixingResult<T> Success<T>(
            AutoMixingContext<T> context, 
            IEnumerable<T> mixedTracks,
            IEnumerable<T> unknownTracks)
            where T : IAutoMixable
        {
            return new AutoMixingResult<T>(
                mixedTracks: mixedTracks,
                context: context,
                unknownTracks: unknownTracks,
                isSuccess: true);
        }

        public static AutoMixingResult<T> Failure<T>(AutoMixingContext<T> context)
            where T : IAutoMixable
        {
            return new AutoMixingResult<T>(
                mixedTracks: context.TracksToMix, 
                context: context, 
                unknownTracks: Enumerable.Empty<T>(),
                isSuccess: false);
        }
    }

    public class AutoMixingResult<T> where T : IAutoMixable
    {
        public AutoMixingResult(
            IEnumerable<T> mixedTracks, 
            AutoMixingContext<T> context,
            IEnumerable<T> unknownTracks,
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
        public IList<T> MixedTracks { get; private set; }
        public IEnumerable<T> UnknownTracks { get; private set; }
        public AutoMixingContext<T> Context { get; private set; }
    }
}