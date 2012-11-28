using MixPlanner.DomainModel;

namespace MixPlanner.Specs
{
    public static class TestMixes
    {
        public static Mix GetRandomMix()
        {
            return new Mix
                       {
                           TestTracks.Get(HarmonicKey.RandomKey()),
                           TestTracks.Get(HarmonicKey.RandomKey()),
                           TestTracks.Get(HarmonicKey.RandomKey()),
                           TestTracks.Get(HarmonicKey.RandomKey()),
                           TestTracks.Get(HarmonicKey.RandomKey())
                       };
        }
    }
}