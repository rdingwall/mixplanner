using System;

namespace MixPlanner.Loader
{
    [Serializable]
    public class UnsupportedFileTypeException : Exception
    {
        public UnsupportedFileTypeException(string filename)
            : base(String.Format("Cannot load '{0}' because it is an unsupported file type.", filename))
        {
        }
    }
}