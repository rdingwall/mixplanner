using MixPlanner.CommandLine.DomainModel;

namespace MixPlanner.Specs
{
    public static class TestMixes
    {
        public static Mix GetRandomMix()
        {
            return new Mix
                       {
                           TestTracks.Get(Key.RandomKey()),
                           TestTracks.Get(Key.RandomKey()),
                           TestTracks.Get(Key.RandomKey()),
                           TestTracks.Get(Key.RandomKey()),
                           TestTracks.Get(Key.RandomKey())
                       };
        }
    }
}