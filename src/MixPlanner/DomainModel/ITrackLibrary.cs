using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MixPlanner.DomainModel
{
    public interface ITrackLibrary
    {
        Task<IEnumerable<Track>> ImportAsync(string filename);
        Task<IEnumerable<Track>> ImportAsync(IEnumerable<string> filenames);
        Task<IEnumerable<Track>> ImportDirectoryAsync(string directoryName);

        void Remove(Track track);
        void RemoveRange(IEnumerable<Track> tracks);
        IEnumerable<Tuple<Track, double>> GetRecommendations(MixItem mixItem);
    }
}