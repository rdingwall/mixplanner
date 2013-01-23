using System;
using System.IO;

namespace MixPlanner.Loader
{
    public interface IWavReader
    {
        bool TryRead(string filename, out Tag track);
    }

    public class WavReader : IWavReader
    {
        readonly IKeyBpmFilenameParser filenameParser;

        public WavReader(
            IKeyBpmFilenameParser filenameParser)
        {
            if (filenameParser == null) throw new ArgumentNullException("filenameParser");
            this.filenameParser = filenameParser;
        }

        public bool TryRead(string filename, out Tag track)
        {
            if (filename == null) throw new ArgumentNullException("filename");

            if (!IsWav(filename))
            {
                track = null;
                return false;
            }

            string key;
            string bpm;
            if (filenameParser.TryParse(filename, out key, out bpm))
            {
                track = new Tag
                            {
                                InitialKey = key,
                                Bpm = bpm,
                                Artist = TrackDefaults.UnknownArtist,
                                Title = Path.GetFileNameWithoutExtension(filename)
                            };
                return true;
            }

            track = null;
            return false;
        }

        static bool IsWav(string filename)
        {
            var extension = Path.GetExtension(filename);

            if (extension == null)
                return false;

            return extension.EndsWith("wav", 
                StringComparison.CurrentCultureIgnoreCase);
        }
    }
}