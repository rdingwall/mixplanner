using System.Threading.Tasks;
using MixPlanner.Configuration;

namespace MixPlanner.Storage
{
    public interface IConfigStorage
    {
        Task SaveAsync(Config config);
        Task<Config> LoadConfigAsync();
    }
}