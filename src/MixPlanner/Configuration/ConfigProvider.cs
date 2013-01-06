using System;
using System.Threading.Tasks;
using MixPlanner.Events;
using MixPlanner.Storage;

namespace MixPlanner.Configuration
{
    public interface IConfigProvider
    {
        Config Config { get; }
        Task Initialize();
    }

    public class ConfigProvider : IConfigProvider
    {
        readonly IConfigStorage storage;
        Config config;

        public ConfigProvider(
            IDispatcherMessenger messenger,
            IConfigStorage storage)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (storage == null) throw new ArgumentNullException("storage");
            messenger.Register<ConfigSavedEvent>(this, e => Config = e.Config);
            this.storage = storage;
        }

        public async Task Initialize()
        {
            Config = await storage.GetConfig();
        }

        public Config Config
        {
            get
            {
                if (config == null)
                    throw new InvalidOperationException("MixPlanner config not loaded yet!");

                return config;
            }
            private set { config = value; }
        }
    }
}