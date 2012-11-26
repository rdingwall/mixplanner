using System.Collections.Generic;

namespace MixPlanner.App.DomainModel
{
    public interface ITrackLibrary
    {
        IEnumerable<Track> Tracks { get; } 

        void Import(string filename);
        void Import(IEnumerable<string> filenames);
        void ImportDirectory(string directoryName);

        void Remove(Track track);
    }
}