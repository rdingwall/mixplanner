using System.Threading.Tasks;
using MixPlanner.Configuration;

namespace MixPlanner.Specs
{
    public class TestConfigProvider : Config, IConfigProvider
    {
        public Config Config { get { return this; } }

        public Task InitializeAsync()
        {
            return Task.FromResult(this);
        }

        public Task SaveAsync(Config config)
        {
            throw new System.NotImplementedException();
        }
    }
}