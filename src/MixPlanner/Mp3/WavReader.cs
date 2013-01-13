using System;
using System.IO;
using MixPlanner.DomainModel;

namespace MixPlanner.Mp3
{
    public interface IWavReader
    {
        bool TryRead(string filename, out Id3Tag track);
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

        public bool TryRead(string filename, out Id3Tag track)
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
                track = new Id3Tag
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