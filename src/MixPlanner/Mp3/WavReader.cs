using System;
using System.IO;

namespace MixPlanner.Mp3
{
    public class WavReader : IId3Reader
    {
        public bool TryRead(string filename, out Id3Tag track)
        {
            if (filename == null) throw new ArgumentNullException("filename");

            if (!IsWav(filename))
            {
                track = null;
                return false;
            }

            track = null;
            return false;
        }

        static bool IsWav(string filename)
        {
            return String.Equals(Path.GetExtension(filename), "wav", 
                StringComparison.CurrentCultureIgnoreCase);
        }
    }
}