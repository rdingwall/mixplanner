﻿using MixPlanner.DomainModel;
using MixPlanner.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MixPlanner.Configuration
{
    public class Config
    {
        static readonly string version = FileVersion.CurrentVersion;

        [JsonConverter(typeof(StringEnumConverter))]
        public HarmonicKeyDisplayMode HarmonicKeyDisplayMode { get; set; }

        public bool RestrictBpmCompatibility { get; set; }
        public bool StripMixedInKeyPrefixes { get; set; }
        public bool SuggestBpmAdjustedTracks { get; set; }
        public bool AutoAdjustBpm { get; set; }
        public bool ParseKeyAndBpmFromFilename { get; set; }

        // For serialization only
        public string Version { get { return version; } }

        public bool ShouldSuggestBpmAdjustments()
        {
            // BPM adjustment suggestions are pointless if BPM compatibility
            // is not being enforced.
            return RestrictBpmCompatibility && SuggestBpmAdjustedTracks;
        }

        public bool ShouldAutoAdjustBpms()
        {
            // Auto adjustments don't make sense if they weren't recommended
            // and shown to the user first.
            return ShouldSuggestBpmAdjustments() && AutoAdjustBpm;
        }

        public static readonly Config DefaultConfig = 
            new Config
                {
                    // Awesome features should be enabled by default..!
                    HarmonicKeyDisplayMode = HarmonicKeyDisplayMode.KeyCode,
                    RestrictBpmCompatibility = true,
                    StripMixedInKeyPrefixes = true,
                    SuggestBpmAdjustedTracks = true,
                    AutoAdjustBpm = true,
                    ParseKeyAndBpmFromFilename = true
                };

    }
}