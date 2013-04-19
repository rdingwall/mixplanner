using MixPlanner.DomainModel;
using Rhino.Mocks;

namespace MixPlanner.Specs
{
    public static class TestMixes
    {
        public static Mix CreateRandomMix(int numberOfTracks = 5)
        {
            var mix = CreateEmptyMix();

            for (int i = 0; i < numberOfTracks; i++)
                mix.Add(TestTracks.CreateRandomTrack(HarmonicKey.RandomKey()));
            
            return mix;
        }

        public static Mix Create(params HarmonicKey[] keys)
        {
            var mix = CreateEmptyMix();

            foreach (HarmonicKey key in keys)
                mix.Add(TestTracks.CreateRandomTrack(key));

            return mix;
        }

        public static Mix CreateEmptyMix()
        {
            return new Mix(
                MockRepository.GenerateMock<IDispatcherMessenger>(),
                new ActualTransitionDetector(TestMixingStrategies.AllStrategies),
                new DummyPlaybackSpeedAdjuster());
        }
    }
}