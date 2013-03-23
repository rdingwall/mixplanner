using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel.AutoMixing
{
    public class AutoMixingResult<T> where T : IAutoMixable
    {
        public AutoMixingResult(
            IEnumerable<T> mixedTracks,
            AutoMixingContext<T> context)
            : this(mixedTracks, context, unknownTracks: Enumerable.Empty<T>())
        {
        }

        public AutoMixingResult(
            IEnumerable<T> mixedTracks, 
            AutoMixingContext<T> context,
            IEnumerable<T> unknownTracks)
        {
            if (mixedTracks == null) throw new ArgumentNullException("mixedTracks");
            if (unknownTracks == null) throw new ArgumentNullException("unknownTracks");
            if (context == null) throw new ArgumentNullException("context");
            MixedTracks = mixedTracks;
            UnknownTracks = unknownTracks;
            Context = context;
        }

        public IEnumerable<T> MixedTracks { get; private set; }
        public IEnumerable<T> UnknownTracks { get; private set; }
        public AutoMixingContext<T> Context { get; private set; }
    }
}