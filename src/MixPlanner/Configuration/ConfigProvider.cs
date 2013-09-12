using System;
using System.IO;
using System.Threading.Tasks;
using MixPlanner.Events;
using MixPlanner.IO.ConfigFiles;
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
        readonly string filename;
        readonly IDispatcherMessenger messenger;
        readonly IConfigReader reader;
        readonly IConfigWriter writer;
        Config config;

        public ConfigProvider(IDispatcherMessenger messenger,
                              IConfigReader reader,
                              IConfigWriter writer)
            : this(MixPlannerPaths.ConfigFile, messenger, reader, writer) {}

        public ConfigProvider(
            string filename,
            IDispatcherMessenger messenger,
            IConfigReader reader,
            IConfigWriter writer)
        {
            if (filename == null) throw new ArgumentNullException("filename");
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (reader == null) throw new ArgumentNullException("reader");
            if (writer == null) throw new ArgumentNullException("writer");
            messenger.Register<ConfigSavedEvent>(this, e => Config = e.Config);
            this.filename = filename;
            this.messenger = messenger;
            this.reader = reader;
            this.writer = writer;
        }

        public async Task InitializeAsync()
        {
            config = await reader.ReadAsync(filename);
            
            if (config == null)
            {
                log.Debug("Initializing MixPlanner with default config settings.");
                config = Config.DefaultConfig;
                await writer.WriteAsync(config, filename);
            }
        }

        public async Task SaveAsync(Config config)
        {
            Config = config;
            await writer.WriteAsync(Config, filename);
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