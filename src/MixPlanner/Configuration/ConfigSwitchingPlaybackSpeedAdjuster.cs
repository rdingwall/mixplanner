using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Configuration
{
    public class ConfigSwitchingPlaybackSpeedAdjuster : IPlaybackSpeedAdjuster
    {
        readonly IConfigProvider configProvider;
        readonly IPlaybackSpeedAdjuster impl;

        public ConfigSwitchingPlaybackSpeedAdjuster(
            IConfigProvider configProvider,
            IPlaybackSpeedAdjuster impl)
        {
            if (configProvider == null) throw new ArgumentNullException("configProvider");
            if (impl == null) throw new ArgumentNullException("impl");
            this.configProvider = configProvider;
            this.impl = impl;
        }

        public double GetSuggestedIncrease(PlaybackSpeed first, PlaybackSpeed second)
        {
            return IsBpmChangeSuggestionAllowed() ? impl.GetSuggestedIncrease(first, second) : 0;
        }

        bool IsBpmChangeSuggestionAllowed()
        {
            var config = configProvider.Config;
            return config.RestrictBpmCompatibility 
                && config.SuggestBpmAdjustedTracks;
        }

        bool IsBpmAutoAdjustmentAllowed()
        {
            var config = configProvider.Config;
            return IsBpmChangeSuggestionAllowed() && config.AutoAdjustBpm;
        }

        public PlaybackSpeed AutoAdjust(PlaybackSpeed first, PlaybackSpeed second)
        {
            return IsBpmAutoAdjustmentAllowed() ? impl.AutoAdjust(first, second) : second;
        }
    }
}