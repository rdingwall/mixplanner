namespace MixPlanner.DomainModel
{
    public class Configuration
    {
        public HarmonicKeyDisplayMode HarmonicKeyDisplayMode { get; set; }
        public bool RestrictBpmCompatibility { get; set; }
        public bool StripMixedInKeyPrefixes { get; set; }


        public static readonly Configuration DefaultConfiguration = 
            new Configuration
                {
                    HarmonicKeyDisplayMode = HarmonicKeyDisplayMode.Camelot,
                    RestrictBpmCompatibility = true,
                    StripMixedInKeyPrefixes = true
                };
    }
}