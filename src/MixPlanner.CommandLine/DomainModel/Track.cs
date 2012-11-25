using System;
using System.IO;

namespace MixPlanner.CommandLine.DomainModel
{
    public class Track : IEquatable<Track>
    {
        public Track(string displayName, Key key, string fileName)
        {
            if (displayName == null) throw new ArgumentNullException("displayName");
            if (key == null) throw new ArgumentNullException("key");
            if (fileName == null) throw new ArgumentNullException("fileName");
            DisplayName = displayName;
            Key = key;
            File = new FileInfo(fileName);
        }

        public string DisplayName { get; private set; }
        public Key Key { get; private set; }
        public FileInfo File { get; set; }

        public override string ToString()
        {
            return String.Format("{0} - {1}", DisplayName, Key);
        }

        public bool Equals(Track other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(File, other.File);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Track) obj);
        }

        public override int GetHashCode()
        {
            return (File != null ? File.GetHashCode() : 0);
        }
    }
}
