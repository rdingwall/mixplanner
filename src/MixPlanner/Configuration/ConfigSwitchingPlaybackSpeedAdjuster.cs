﻿using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Configuration
{
    public class ConfigSwitchingPlaybackSpeedAdjuster : ILimitingPlaybackSpeedAdjuster
    {
        readonly IConfigProvider configProvider;
        readonly ILimitingPlaybackSpeedAdjuster impl;

        public ConfigSwitchingPlaybackSpeedAdjuster(
            IConfigProvider configProvider,
            ILimitingPlaybackSpeedAdjuster impl)
        {
            if (configProvider == null) throw new ArgumentNullException("configProvider");
            if (impl == null) throw new ArgumentNullException("impl");
            this.configProvider = configProvider;
            this.impl = impl;
        }

        public double GetSuggestedIncrease(PlaybackSpeed first, PlaybackSpeed second)
        {
            var config = configProvider.Config;
            return config.ShouldSuggestBpmAdjustments() ? impl.GetSuggestedIncrease(first, second) : 0;
        }

        public PlaybackSpeed AutoAdjust(PlaybackSpeed first, PlaybackSpeed second)
        {
            var config = configProvider.Config;
            return config.ShouldAutoAdjustBpms() ? impl.AutoAdjust(first, second) : second;
        }
    }
}