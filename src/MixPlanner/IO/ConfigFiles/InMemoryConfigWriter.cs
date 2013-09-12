using System;
using System.Threading.Tasks;
using MixPlanner.Configuration;

namespace MixPlanner.IO.ConfigFiles
{
    public class InMemoryConfigWriter : IConfigWriter, IConfigReader
    {
        Config config;

        public async Task WriteAsync(Config config, string filename)
        {
            if (config == null) throw new ArgumentNullException("config");
            this.config = config;
        }

        public async Task<Config> ReadAsync(string filename)
        {
            return await Task.Run(() => config);
        }
    }
}