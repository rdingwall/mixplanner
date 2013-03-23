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

        Task RemoveAsync(Track track);
        Task RemoveRangeAsync(IEnumerable<Track> tracks);
        IEnumerable<Tuple<Track, Transition>> GetRecommendations(IMixItem mixItem);
        Task SaveAsync(Track track);
    }
}