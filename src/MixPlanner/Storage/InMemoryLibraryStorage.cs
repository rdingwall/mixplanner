using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MixPlanner.DomainModel;

namespace MixPlanner.Storage
{
    public class InMemoryLibraryStorage : ILibraryStorage
    {
        readonly IList<Track> tracks;

        public InMemoryLibraryStorage()
        {
            tracks = new List<Track>();
        }

        public IEnumerable<Track> Tracks { get { return tracks; } }

        public Task<IEnumerable<Track>> LoadAllTracksAsync()
        {
            return Task.Run(() => Enumerable.Empty<Track>());
        }

        public async Task AddTrackAsync(Track track)
        {
            await Task.Run(() => tracks.Add(track));
        }

        public async Task RemoveTrackAsync(Track track)
        {
            await Task.Run(() => tracks.Remove(track));
        }

        public async Task UpdateTrackAsync(Track track)
        {
            await Task.Yield();
        }
    }
}