using System;
using MixPlanner.Configuration;

namespace MixPlanner.Events
{
    public class ConfigSavedEvent
    {
        public ConfigSavedEvent(Config config)
        {
            if (config == null) throw new ArgumentNullException("config");
            Config = config;
        }

        public Config Config { get; private set; }
    }
}