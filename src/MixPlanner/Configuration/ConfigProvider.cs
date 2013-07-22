using System;
using System.Threading.Tasks;
using MixPlanner.Events;
using MixPlanner.Storage;
using log4net;

namespace MixPlanner.Configuration
{
    public interface IConfigProvider
    {
        Config Config { get; }
        Task InitializeAsync();
        Task SaveAsync(Config config);
    }

    public class ConfigProvider : IConfigProvider
    {
        static readonly ILog log = LogManager.GetLogger(typeof (ConfigProvider));
        readonly IDispatcherMessenger messenger;
        readonly IConfigStorage storage;
        Config config;

        public ConfigProvider(
            IDispatcherMessenger messenger,
            IConfigStorage storage)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (storage == null) throw new ArgumentNullException("storage");
            messenger.Register<ConfigSavedEvent>(this, e => Config = e.Config);
            this.messenger = messenger;
            this.storage = storage;
        }

        public async Task InitializeAsync()
        {
            config = await storage.LoadConfigAsync();
            
            if (config == null)
            {
                log.Debug("Initializing MixPlanner with default config settings.");
                config = Config.DefaultConfig;
                await storage.SaveAsync(config);
            }
        }

        public async Task SaveAsync(Config config)
        {
            Config = config;
            await storage.SaveAsync(Config);
            await Task.Run(() => messenger.SendToUI(new ConfigSavedEvent(config)));
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