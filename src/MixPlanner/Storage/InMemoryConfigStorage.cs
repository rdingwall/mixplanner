using System;
using System.Threading.Tasks;
using MixPlanner.Configuration;

namespace MixPlanner.Storage
{
    public class InMemoryConfigStorage : IConfigStorage
    {
        Config config;

        public async Task SaveAsync(Config config)
        {
            if (config == null) throw new ArgumentNullException("config");
            this.config = config;
        }

        public async Task<Config> LoadConfigAsync()
        {
            return await Task.Run(() => config);
        }
    }
}