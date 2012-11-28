using System.Collections.Generic;

namespace MixPlanner.DomainModel
{
    public interface ITrackLibrary
    {
        void Import(string filename);
        void Import(IEnumerable<string> filenames);
        void ImportDirectory(string directoryName);

        void Remove(Track track);
    }
}