using System;
using System.IO;

namespace MixPlanner.DomainModel
{
    public class Track : IEquatable<Track>
    {
        public Track(
            string artist, 
            string title, 
            HarmonicKey originalKey, 
            string fileName,
            double originalBpm)
        {
            if (artist == null) throw new ArgumentNullException("artist");
            if (title == null) throw new ArgumentNullException("title");
            if (originalKey == null) throw new ArgumentNullException("originalKey");
            if (fileName == null) throw new ArgumentNullException("fileName");
            Artist = artist;
            Title = title;
            OriginalKey = originalKey;
            Filename = fileName;
            OriginalBpm = originalBpm;

            Label = "";
            Genre = "";
            Year = "";

            SearchData = String.Concat(artist,
                                       title,
                                       Path.GetFileNameWithoutExtension(fileName),
                                       originalBpm.ToString(),
                                       originalKey.ToString());
        }

        public string SearchData { get; private set; }

        public string Artist { get; set; }
        public string Title { get; set; }
        public HarmonicKey OriginalKey { get; set; }
        public string Filename { get; set; }

        public FileInfo File
        {
            get { return String.IsNullOrEmpty(Filename) ? null : new FileInfo(Filename); }
        }

        public double OriginalBpm { get; set; }

        // Optional properties
        public string Label { get; set; }
        public string Genre { get; set; }
        public string Year { get; set; }

        public bool HasFilename
        {
            get { return !String.IsNullOrWhiteSpace(Filename); }
        }

        public PlaybackSpeed GetDefaultPlaybackSpeed()
        {
            return new PlaybackSpeed(OriginalKey, OriginalBpm);
        }

        public override string ToString()
        {
            return String.Format("{0} - {1}", Artist, OriginalKey);
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
