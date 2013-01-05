using System.Threading.Tasks;
using MixPlanner.DomainModel;

namespace MixPlanner.Storage
{
    public interface IConfigurationStorage
    {
        Task Save(Configuration configuration);
        Task<Configuration> GetConfiguration();
    }
}