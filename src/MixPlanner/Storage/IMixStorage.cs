using System.Threading.Tasks;
using MixPlanner.DomainModel;

namespace MixPlanner.Storage
{
    public interface IMixStorage
    {
        Task SaveAsync(IMix mix, string filename);
        Task<IMix> OpenAsync(string filename); 
    }
}