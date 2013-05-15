using System.Collections.Generic;
using System.Threading.Tasks;
using MixPlanner.DomainModel;

namespace MixPlanner.Storage
{
    public interface ILibraryStorage
    {
        Task<IEnumerable<Track>> FetchAllAsync();
        Task AddAsync(Track track);
        Task RemoveAsync(Track track);
        Task UpdateAsync(Track track);
    }
}