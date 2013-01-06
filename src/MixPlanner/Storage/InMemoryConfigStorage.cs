using System;
using System.Threading.Tasks;
using MixPlanner.Configuration;
using MixPlanner.Events;

namespace MixPlanner.Storage
{
    public class InMemoryConfigStorage : IConfigStorage
    {
        Config config;

        readonly IDispatcherMessenger messenger;

        public InMemoryConfigStorage(IDispatcherMessenger messenger)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.messenger = messenger;
            config = Config.DefaultConfig;
        }

        public async Task SaveAsync(Config config)
        {
            if (config == null) throw new ArgumentNullException("config");
            this.config = config;
            await Task.Run(() => messenger.SendToUI(new ConfigSavedEvent(config)));
        }

        public async Task<Config> GetConfigAsync()
        {
            return await Task.Run(() => config);
        }
    }
}