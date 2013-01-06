using MixPlanner.DomainModel;

namespace MixPlanner.Configuration
{
    public class Config
    {
        public HarmonicKeyDisplayMode HarmonicKeyDisplayMode { get; set; }
        public bool RestrictBpmCompatibility { get; set; }
        public bool StripMixedInKeyPrefixes { get; set; }


        public static readonly Config DefaultConfig = 
            new Config
                {
                    HarmonicKeyDisplayMode = HarmonicKeyDisplayMode.Camelot,
                    RestrictBpmCompatibility = true,
                    StripMixedInKeyPrefixes = true
                };
    }
}