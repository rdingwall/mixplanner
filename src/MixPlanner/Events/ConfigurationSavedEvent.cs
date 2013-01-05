using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class ConfigurationSavedEvent
    {
        public ConfigurationSavedEvent(Configuration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            Configuration = configuration;
        }

        public Configuration Configuration { get; private set; }
    }
}