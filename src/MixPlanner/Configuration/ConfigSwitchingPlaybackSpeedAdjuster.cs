using System;
using System.Collections.Generic;
using MixPlanner.DomainModel;

namespace MixPlanner.Configuration
{
    public class ConfigSwitchingPlaybackSpeedAdjuster : LimitingPlaybackSpeedAdjuster
    {
        readonly IConfigProvider configProvider;

        public ConfigSwitchingPlaybackSpeedAdjuster(
            IConfigProvider configProvider)
        {
            if (configProvider == null) throw new ArgumentNullException("configProvider");
            this.configProvider = configProvider;
        }

        public override double GetSuggestedIncrease(PlaybackSpeed first, double targetBpm)
        {
            var config = configProvider.Config;
            return config.ShouldSuggestBpmAdjustments() ? base.GetSuggestedIncrease(first, targetBpm) : 0;
        }

        public override PlaybackSpeed AutoAdjust(PlaybackSpeed track, double targetBpm)
        {
            var config = configProvider.Config;
            return config.ShouldAutoAdjustBpms() ? base.AutoAdjust(track, targetBpm) : track;
        }

        public override void AutoAdjustAll(IEnumerable<PlaybackSpeed> tracks, double targetBpm)
        {
            var config = configProvider.Config;
            if (config.ShouldAutoAdjustBpms())
                base.AutoAdjustAll(tracks, targetBpm);
        }
    }
}