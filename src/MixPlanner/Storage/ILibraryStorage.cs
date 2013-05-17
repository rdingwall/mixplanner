using System.Collections.Generic;
using System.Threading.Tasks;
using MixPlanner.DomainModel;

namespace MixPlanner.Storage
{
    public interface ILibraryStorage
    {
        Task<IEnumerable<Track>> LoadAllTracksAsync();
        Task AddTrackAsync(Track track);
        Task RemoveTrackAsync(Track track);
        Task UpdateTrackAsync(Track track);
    }
}