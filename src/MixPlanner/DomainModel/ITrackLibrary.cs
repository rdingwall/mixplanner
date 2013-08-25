using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MixPlanner.DomainModel
{
    public interface ITrackLibrary
    {
        Task<IEnumerable<Track>> ImportAsync(string filename,
            CancellationToken cancellationToken, IProgress<string> progress);
        Task<IEnumerable<Track>> ImportAsync(IEnumerable<string> filenames, 
            CancellationToken cancellationToken, IProgress<string> progress);
        Task<IEnumerable<Track>> ImportDirectoryAsync(string directoryName,
            CancellationToken cancellationToken, IProgress<string> progress);

        Task InitializeAsync();
        Task RemoveAsync(Track track);
        Task RemoveRangeAsync(IEnumerable<Track> tracks);
        IEnumerable<Tuple<Track, Transition>> GetRecommendations(IMixItem mixItem);
        Task SaveAsync(Track track);
        Track GetById(string id);

        bool TryGetById(string id, out Track track);
    }
}