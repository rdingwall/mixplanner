using MixPlanner.DomainModel;

namespace MixPlanner.Configuration
{
    public class Config
    {
        public HarmonicKeyDisplayMode HarmonicKeyDisplayMode { get; set; }
        public bool RestrictBpmCompatibility { get; set; }
        public bool StripMixedInKeyPrefixes { get; set; }
        public bool SuggestBpmAdjustedTracks { get; set; }
        public bool AutoAdjustBpm { get; set; }
        public bool ParseKeyAndBpmFromFilename { get; set; }

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