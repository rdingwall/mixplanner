using System;
using System.Collections.Generic;
using System.Linq;

namespace MixPlanner.DomainModel.AutoMixing
{
    public class AutoMixingBucket<T> : IAutoMixable, IEquatable<AutoMixingBucket<T>>
    {
        public AutoMixingBucket(IEnumerable<T> tracks, HarmonicKey harmonicKey)
        {
            if (tracks == null) throw new ArgumentNullException("tracks");
            if (harmonicKey == null) throw new ArgumentNullException("harmonicKey");
            if (!tracks.Any()) throw new ArgumentException("Bucket cannot be empty.", "tracks");
            Tracks = tracks;
            PlaybackSpeed = new PlaybackSpeed(harmonicKey, 128);
        }

        public IEnumerable<T> Tracks { get; private set; }
        public PlaybackSpeed PlaybackSpeed { get; private set; }
        public bool IsUnknownKeyOrBpm { get { return false; } }

        public bool Equals(AutoMixingBucket<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(PlaybackSpeed, other.PlaybackSpeed);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AutoMixingBucket<T>) obj);
        }

        public override int GetHashCode()
        {
            return (PlaybackSpeed != null ? PlaybackSpeed.GetHashCode() : 0);
        }
    }
}