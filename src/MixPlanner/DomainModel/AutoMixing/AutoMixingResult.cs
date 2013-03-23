using System;
using System.Collections.Generic;

namespace MixPlanner.DomainModel.AutoMixing
{
    public class AutoMixingResult<T> where T : IAutoMixable
    {
        public AutoMixingResult(
            IEnumerable<T> mixedTracks, 
            IEnumerable<T> unknownTracks, 
            AutoMixingContext<T> context)
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