using System.Collections.Generic;
using MixPlanner.DomainModel;

namespace MixPlanner.Storage
{
    public interface ILibraryStorage
    {
        IEnumerable<Track> Tracks { get; }
        void Add(Track track);
        void Remove(Track track);
    }
}