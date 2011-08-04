using System;

namespace Julana.CommandLine.DomainModel
{
    public class Track : IEquatable<Track>
    {
        public Track(string name, Key key)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (key == null) throw new ArgumentNullException("key");
            Name = name;
            Key = key;
        }

        public string Name { get; private set; }
        public Key Key { get; private set; }

        public override string ToString()
        {
            return String.Format("{0} - {1}", Name, Key);
        }

        public bool Equals(Track other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name) && Equals(other.Key, Key);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Track)) return false;
            return Equals((Track) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0)*397) ^ (Key != null ? Key.GetHashCode() : 0);
            }
        }
    }
}
