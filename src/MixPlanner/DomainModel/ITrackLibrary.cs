using System.Collections.Generic;

namespace MixPlanner.DomainModel
{
    public interface ITrackLibrary
    {
        IEnumerable<Track> Import(string filename);
        IEnumerable<Track> Import(IEnumerable<string> filenames);
        IEnumerable<Track> ImportDirectory(string directoryName);

        void Remove(Track track);
        void RemoveRange(IEnumerable<Track> tracks);
    }
}