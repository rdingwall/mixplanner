using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel.AutoMixing
{
    public class AutoMixingBucket : IEnumerable<IMixItem>
    {
        private readonly IEnumerable<IMixItem> tracks;

        public AutoMixingBucket(HarmonicKey harmonicKey)
            : this(Enumerable.Empty<IMixItem>(), harmonicKey)
        {
        }

        public AutoMixingBucket(IEnumerable<IMixItem> tracks, HarmonicKey harmonicKey)
        {
            if (tracks == null) throw new ArgumentNullException("tracks");
            this.tracks = tracks;
            ActualKey = harmonicKey;
        }

        public bool ContainsKey(HarmonicKey key)
        {
            return ActualKey.Equals(key);
        }

        public HarmonicKey ActualKey { get; private set; }

        public IEnumerator<IMixItem> GetEnumerator()
        {
            return tracks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return ActualKey.ToString();
        }
    }
}