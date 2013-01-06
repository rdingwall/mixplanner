using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Configuration
{
    public class ConfigSwitchingBpmRangeChecker : IBpmRangeChecker
    {
        readonly IConfigProvider configProvider;
        readonly IBpmRangeChecker impl;

        public ConfigSwitchingBpmRangeChecker(
            IConfigProvider configProvider,
            IBpmRangeChecker impl)
        {
            if (configProvider == null) throw new ArgumentNullException("configProvider");
            if (impl == null) throw new ArgumentNullException("impl");
            this.configProvider = configProvider;
            this.impl = impl;
        }

        public bool IsWithinBpmRange(PlaybackSpeed first, PlaybackSpeed second)
        {
            var config = configProvider.Config;
            return !config.RestrictBpmCompatibility || impl.IsWithinBpmRange(first, second);
        }
    }
}