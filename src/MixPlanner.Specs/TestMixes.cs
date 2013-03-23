using MixPlanner.DomainModel;
using Rhino.Mocks;

namespace MixPlanner.Specs
{
    public static class TestMixes
    {
        public static Mix GetRandomMix(int numberOfTracks = 5)
        {
            var mix = GetEmptyMix();

            for (int i = 0; i < numberOfTracks; i++)
                mix.Add(TestTracks.GetRandomTrack(HarmonicKey.RandomKey()));
            
            return mix;
        }

        public static Mix Create(params HarmonicKey[] keys)
        {
            var mix = GetEmptyMix();

            foreach (HarmonicKey key in keys)
                mix.Add(TestTracks.GetRandomTrack(key));

            return mix;
        }

        public static Mix GetEmptyMix()
        {
            return new Mix(
                MockRepository.GenerateMock<IDispatcherMessenger>(),
                new ActualTransitionDetector(TestMixingStrategies.AllStrategies),
                new DummyPlaybackSpeedAdjuster());
        }
    }
}