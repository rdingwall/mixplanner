using System;
using System.Threading.Tasks;
using MixPlanner.DomainModel;
using MixPlanner.Events;

namespace MixPlanner.Storage
{
    public class InMemoryConfigurationStorage : IConfigurationStorage
    {
        Configuration configuration;

        readonly IDispatcherMessenger messenger;

        public InMemoryConfigurationStorage(IDispatcherMessenger messenger)
        {
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.messenger = messenger;
            configuration = Configuration.DefaultConfiguration;
        }

        public async Task Save(Configuration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            this.configuration = configuration;
            await Task.Run(() => messenger.SendToUI(new ConfigurationSavedEvent(configuration)));
        }

        public async Task<Configuration> GetConfiguration()
        {
            return await Task.Run(() => configuration);
        }
    }
}