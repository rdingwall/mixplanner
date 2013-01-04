using System.Collections.Generic;
using System.Threading.Tasks;

namespace MixPlanner.DomainModel
{
    public interface ITrackLibrary
    {
        IEnumerable<Track> Import(string filename);
        IEnumerable<Track> Import(IEnumerable<string> filenames);
        IEnumerable<Track> ImportDirectory(string directoryName);
        
        Task<IEnumerable<Track>> ImportAsync(string filename);
        Task<IEnumerable<Track>> ImportAsync(IEnumerable<string> filenames);
        Task<IEnumerable<Track>> ImportDirectoryAsync(string directoryName);

        void Remove(Track track);
        void RemoveRange(IEnumerable<Track> tracks);
    }
}