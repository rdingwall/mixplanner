using System;
using System.Threading.Tasks;
using MixPlanner.Events;
using MixPlanner.Storage;

namespace MixPlanner.DomainModel
{
    public interface IConfigurationProvider
    {
        Configuration Configuration { get; }
    }

    public class ConfigurationProvider : IConfigurationProvider
    {
        readonly IConfigurationStorage storage;
        Configuration configuration;

        public ConfigurationProvider(
            IDispatcherMessenger messenger,
            IConfigurationStorage storage)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (storage == null) throw new ArgumentNullException("storage");
            messenger.Register<ConfigurationSavedEvent>(this, e => Configuration = e.Configuration);
            this.storage = storage;
            Task.Run(() => Initialize());
        }

        async Task Initialize()
        {
            Configuration = await storage.GetConfiguration();
        }

        public Configuration Configuration
        {
            get
            {
                if (configuration == null)
                    throw new InvalidOperationException("Configuration not loaded yet!");

                return configuration;
            }
            private set { configuration = value; }
        }
    }
}