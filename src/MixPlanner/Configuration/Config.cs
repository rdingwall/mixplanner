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

        public static readonly Config DefaultConfig = 
            new Config
                {
                    // Awesome features should be enabled by default..!
                    HarmonicKeyDisplayMode = HarmonicKeyDisplayMode.Camelot,
                    RestrictBpmCompatibility = true,
                    StripMixedInKeyPrefixes = true,
                    SuggestBpmAdjustedTracks = true,
                    AutoAdjustBpm = true
                };
    }
}