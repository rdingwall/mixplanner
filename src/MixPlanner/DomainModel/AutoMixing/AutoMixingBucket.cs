using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel.AutoMixing
{
    public class AutoMixingBucket : IEquatable<AutoMixingBucket>, IEnumerable<IMixItem>
    {
        private readonly IEnumerable<IMixItem> tracks;

        public AutoMixingBucket(HarmonicKey harmonicKey)
            : this(Enumerable.Empty<IMixItem>(), harmonicKey)
        {
        }

        public AutoMixingBucket(IEnumerable<IMixItem> tracks, HarmonicKey harmonicKey)
        {
            if (tracks == null) throw new ArgumentNullException("tracks");
            if (harmonicKey == null) throw new ArgumentNullException("harmonicKey");
            this.tracks = tracks;
            ActualKey = harmonicKey;
        }

        public bool ContainsKey(HarmonicKey key)
        {
            if (key == null) throw new ArgumentNullException("key");
            return ActualKey.Equals(key);
        }

        public HarmonicKey ActualKey { get; private set; }

        public bool Equals(AutoMixingBucket other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(ActualKey, other.ActualKey);
        }

        public IEnumerator<IMixItem> GetEnumerator()
        {
            return tracks.GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AutoMixingBucket) obj);
        }

        public override int GetHashCode()
        {
            return (ActualKey != null ? ActualKey.GetHashCode() : 0);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}