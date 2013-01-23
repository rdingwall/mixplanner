using System;

namespace MixPlanner.Loader
{
    public static class FileNameHelper
    {
        public static bool IsMp3(string filename)
        {
            return HasExtension(filename, "mp3");
        }

        public static bool IsWav(string filename)
        {
            return HasExtension(filename, "wav");
        }

        public static bool HasExtension(string filename, string extension)
        {
            if (extension == null) throw new ArgumentNullException("extension");
            if (filename == null)
                return false;
            
            return filename.EndsWith(extension, 
                StringComparison.CurrentCultureIgnoreCase);
        } 
    }
}