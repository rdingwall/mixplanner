using System;
using System.Linq;
using log4net;
using File = TagLib.Mpeg4.File;

namespace MixPlanner.Loader
{
    public interface IAacReader
    {
        bool TryRead(string filename, out Tag tag);
    }

    public class AacReader : IAacReader
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(AacReader));

        public bool TryRead(string filename, out Tag tag)
        {
            if (filename == null) throw new ArgumentNullException("filename");

            try
            {
                var file = new File(filename);

                LogWarnings(filename, file);

                var t = new Tag();
                PopulateFromTag(file, t);
                PopulateFallbackValues(t);

                if (file.Properties != null)
                    t.Duration = file.Properties.Duration;

                tag = t;
                return true;
            }
            catch (Exception e)
            {
                Log.Error(String.Format("Error reading M4A tag from {0}.", filename), e);
                tag = null;
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

        void PopulateFromTag(File file, Tag tag)
        {
            //if (!file.TagTypes.HasFlag(TagTypes.Id3v2))
            //    return;

            tag.ImageData = GetImageData(file);

            // ID3v2 Tags Reference: http://id3.org/id3v2.4.0-frames
            tag.Artist = JoinPerformers(file.Tag.Performers);
            tag.Title = file.Tag.Title;
            tag.Year = ToStringOrDefault(file.Tag.Year);
            tag.Genre = file.Tag.JoinedGenres;
            tag.Publisher = file.Tag.Grouping;
            tag.Bpm = ToStringOrDefault(file.Tag.BeatsPerMinute);
        }

        void PopulateFallbackValues(Tag tag)
        {
            tag.Artist = tag.Artist ?? TrackDefaults.UnknownArtist;
            tag.Title = tag.Title ?? TrackDefaults.UnknownTitle;
        }

        static string JoinPerformers(string[] performers)
        {
            // taglib-sharp interprets forward slashes (/) as separator but
            // Mixed In Key uses this to denote different keys e.g. 1A/11A
            return String.Join("/", performers);
        }

        static byte[] GetImageData(File file)
        {
            var picture = file.Tag.Pictures.FirstOrDefault(p => p.Description.Contains("FrontCover")) ??
                          file.Tag.Pictures.FirstOrDefault();

            if (picture == null)
                return null;

            return picture.Data.Data;
        }

        string ToStringOrDefault(uint value)
        {
            if (value == 0)
                return null;

            return value.ToString();
        }
    }
}