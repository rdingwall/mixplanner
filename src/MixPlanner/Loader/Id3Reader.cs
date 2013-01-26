using System;
using System.Linq;
using TagLib;
using TagLib.Id3v2;
using log4net;

namespace MixPlanner.Loader
{
    public interface IId3Reader
    {
        bool TryRead(string filename, out Tag track);
    }

    public class Id3Reader : IId3Reader
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(Id3Reader));

        protected virtual File OpenFile(string filename)
        {
            return File.Create(filename);
        }

        public bool TryRead(string filename, out Tag id3Tag)
        {
            if (filename == null) throw new ArgumentNullException("filename");
            try
            {
                var file = OpenFile(filename);

                LogWarnings(filename, file);

                var t = new Tag();

                PopulateFromId3v2(file, t);
                PopulateFromId3v1(file, t);
                PopulateFallbackValues(t);
    
                id3Tag = t;
                return true;
            }
            catch (Exception e)
            {
                Log.Error(String.Format("Error reading ID3 tag from {0}.", filename), e);
                id3Tag = null;
                return false;
            }
        }

        static void LogWarnings(string filename, File file)
        {
            if (!file.PossiblyCorrupt)
                return;

            foreach (var reason in file.CorruptionReasons)
                Log.WarnFormat("{0} is possibly corrupt: {1}", filename, reason);
        }

        void PopulateFallbackValues(Tag tag)
        {
            tag.Artist = tag.Artist ?? TrackDefaults.UnknownArtist;
            tag.Title = tag.Title ?? TrackDefaults.UnknownTitle;
        }

        void PopulateFromId3v2(File file, Tag tag)
        {
            if (!file.TagTypes.HasFlag(TagTypes.Id3v2))
                return;

            var id3v2 = (TagLib.Id3v2.Tag)file.GetTag(TagTypes.Id3v2);
            
            tag.ImageData = GetImageData(id3v2);

            // ID3v2 Tags Reference: http://id3.org/id3v2.4.0-frames
            tag.InitialKey = GetTextFrame(id3v2, "TKEY") ?? GetUserTextFrame(id3v2, "Initial key");
            tag.Artist = JoinPerformers(id3v2.Performers);
            tag.Title = id3v2.Title;
            tag.Year = ToStringOrDefault(id3v2.Year);
            tag.Genre = id3v2.JoinedGenres;
            tag.Publisher = GetTextFrame(id3v2, "TPUB") ?? GetUserTextFrame(id3v2, "Publisher");
            tag.Bpm = ToStringOrDefault(id3v2.BeatsPerMinute) ?? GetUserTextFrame(id3v2, "BPM (beats per minute)");
        }

        static string JoinPerformers(string[] performers)
        {
            // taglib-sharp interprets forward slashes (/) as separator but
            // Mixed In Key uses this to denote different keys e.g. 1A/11A
             return String.Join("/", performers);
        }

        static byte[] GetImageData(TagLib.Id3v2.Tag id3v2)
        {
            var picture = id3v2.Pictures.FirstOrDefault(p => p.Description.Contains("FrontCover")) ??
                          id3v2.Pictures.FirstOrDefault();

            if (picture == null)
                return null;

            return picture.Data.Data;
        }

        string GetTextFrame(TagLib.Id3v2.Tag id3v2, string identifier)
        {
            return id3v2
                .GetFrames<TextInformationFrame>(identifier)
                .SelectMany(f => f.Text)
                .FirstOrDefault();
        }

        string GetUserTextFrame(TagLib.Id3v2.Tag id3v2, string description)
        {
            return id3v2
                .GetFrames<UserTextInformationFrame>()
                .Where(f => String.Equals(f.Description, description, StringComparison.CurrentCultureIgnoreCase))
                .SelectMany(f => f.Text)
                .FirstOrDefault();
        }

        void PopulateFromId3v1(File file, Tag tag)
        {
            if (!file.TagTypes.HasFlag(TagTypes.Id3v1))
                return;

            var id3v1 = (TagLib.Id3v1.Tag) file.GetTag(TagTypes.Id3v1);

            tag.Artist = tag.Artist ?? JoinPerformers(id3v1.Performers);
            tag.Title = tag.Title ?? id3v1.Title;
            tag.Year = tag.Year ?? ToStringOrDefault(id3v1.Year);
            tag.Genre = tag.Genre ?? id3v1.JoinedGenres;
            //tag.Publisher = tag.Publisher ?? id3v1.
            tag.Bpm = tag.Bpm ?? ToStringOrDefault(id3v1.BeatsPerMinute);
        }

        string ToStringOrDefault(uint value)
        {
            if (value == 0)
                return null;

            return value.ToString();
        }
    }
}