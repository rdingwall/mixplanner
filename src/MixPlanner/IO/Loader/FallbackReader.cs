using System;
using System.IO;
using log4net;
using File = TagLib.File;

namespace MixPlanner.IO.Loader
{
    public interface IFallbackReader
    {
        Tag Read(string filename);
    }

    public class FallbackReader : IFallbackReader
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(FallbackReader));

        public Tag Read(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");

            var t = new Tag();

            t.Title = Path.GetFileNameWithoutExtension(filename);
            t.Artist = TrackDefaults.UnknownArtist;
            t.Duration = GetDuration(filename);

            return t;
        }

        static TimeSpan GetDuration(string filename)
        {
            try
            {
                var file = File.Create(filename);
                LogWarnings(filename, file);

                if (file.Properties != null)
                    return file.Properties.Duration;

                return TimeSpan.Zero;
            }
            catch (Exception e)
            {
                Log.Error(String.Format("Error reading media file data from {0}.", filename), e);
                return TimeSpan.Zero;
            }
        }

        static void LogWarnings(string filename, File file)
        {
            if (!file.PossiblyCorrupt)
                return;

            foreach (var reason in file.CorruptionReasons)
                Log.WarnFormat("{0} is possibly corrupt: {1}", filename, reason);
        }
    }
}