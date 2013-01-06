using System.Threading.Tasks;
using MixPlanner.Configuration;
using MixPlanner.DomainModel;

namespace MixPlanner.Storage
{
    public interface IConfigStorage
    {
        Task Save(Config config);
        Task<Config> GetConfig();
    }
}