using System.Collections.Generic;
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

        public void Add(Track track)
        {
            tracks.Add(track);
        }

        public void Remove(Track track)
        {
            tracks.Remove(track);
        }
    }
}