using System;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using MixPlanner.Loader;
using MixPlanner.Storage;

namespace MixPlanner.DomainModel
{
    public class Track : IEquatable<Track>
    {
        public Track(string artist,
                     string title,
                     HarmonicKey originalKey,
                     string fileName,
                     double originalBpm,
                     TimeSpan duration)
            : this(GenerateId(), artist, title, originalKey, fileName, originalBpm, duration)
        {
        }

        static string GenerateId()
        {
            return Guid.NewGuid().ToString("N");
        }

        public Track(
            string id,
            string artist,
            string title,
            HarmonicKey originalKey,
            string fileName,
            double originalBpm,
            TimeSpan duration)
        {
            if (id == null) throw new ArgumentNullException("id");
            if (artist == null) throw new ArgumentNullException("artist");
            if (title == null) throw new ArgumentNullException("title");
            if (fileName == null) throw new ArgumentNullException("fileName");
            Id = id;
            Artist = artist;
            Title = title;
            OriginalKey = originalKey;
            Filename = fileName;
            OriginalBpm = originalBpm;
            Duration = duration;

            Label = "";
            Genre = "";
            Year = "";
        }

        public string Id { get; private set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public HarmonicKey OriginalKey { get; set; }
        public string Filename { get; set; }
        public TrackImageData Images { get; set; }

        public FileInfo File
        {
            get { return String.IsNullOrEmpty(Filename) ? null : new FileInfo(Filename); }
        }

        public double OriginalBpm { get; set; }
        public TimeSpan Duration { get; set; }

        // Optional properties
        public string Label { get; set; }
        public string Genre { get; set; }
        public string Year { get; set; }

        public bool HasFilename
        {
            get { return !String.IsNullOrWhiteSpace(Filename); }
        }

        public bool IsUnknownBpm
        {
            get { return Double.IsNaN(OriginalBpm); }
        }

        public bool IsUnknownKey
        {
            get { return OriginalKey.IsUnknown; }
        }

        public bool HasImages
        {
            get { return Images != null; }
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
            return string.Equals(Id, other.Id);
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
            return Id.GetHashCode();
        }

        public ImageSource GetFullSizeImageSource()
        {
            if (Images == null || Images.FullSize == null)
                return null;

            return Images.FullSize.ImageSource;
        }

        public ImageSource Get24x24ImageSource()
        {
            if (Images == null || Images.Resized24X24 == null)
                return null;

            return Images.Resized24X24.ImageSource;
        }

        public ImageSource Get64x64ImageSource()
        {
            if (Images == null || Images.Resized24X24 == null)
                return null;

            return Images.Resized64X64.ImageSource;
        }

        public byte[] GetFullSizeImageBytes()
        {
            if (Images == null)
                return null;

            return Images.FullSize.Data;
        }

        public static Track FromJson(JsonTrack jsonTrack)
        {
            if (jsonTrack == null) throw new ArgumentNullException("jsonTrack");

            return new Track(id: jsonTrack.Id,
                             artist: jsonTrack.Artist,
                             title: jsonTrack.Title,
                             originalKey: jsonTrack.OriginalKey,
                             fileName: jsonTrack.Filename,
                             originalBpm: jsonTrack.OriginalBpm,
                             duration: jsonTrack.Duration)
                       {
                           Year = jsonTrack.Year,
                           Label = jsonTrack.Label,
                           Genre = jsonTrack.Genre
                       };
        }
    }
}
