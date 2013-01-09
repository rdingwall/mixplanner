using System.Collections.Generic;
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

        public async Task AddAsync(Track track)
        {
            await Task.Run(() => tracks.Add(track));
        }

        public async Task RemoveAsync(Track track)
        {
            await Task.Run(() => tracks.Remove(track));
        }

        public async Task UpdateAsync(Track track)
        {
            await Task.Yield();
        }
    }
}