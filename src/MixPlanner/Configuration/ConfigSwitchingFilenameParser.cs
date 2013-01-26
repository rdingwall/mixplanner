using System;
using MixPlanner.Loader;

namespace MixPlanner.Configuration
{
    public class ConfigSwitchingFilenameParser : IFilenameParser
    {
        readonly IConfigProvider configProvider;
        readonly IFilenameParser impl;

        public ConfigSwitchingFilenameParser(
            IConfigProvider configProvider,
            IFilenameParser impl)
        {
            if (configProvider == null) throw new ArgumentNullException("configProvider");
            if (impl == null) throw new ArgumentNullException("impl");
            this.configProvider = configProvider;
            this.impl = impl;
        }

        public bool TryParse(string filename, out string firstKey, out string bpm)
        {
            var config = configProvider.Config;

            if (!config.ParseKeyAndBpmFromFilename)
            {
                firstKey = null;
                bpm = null;
                return false;
            }

            return impl.TryParse(filename, out firstKey, out bpm);
        }
    }
}