using MixPlanner.DomainModel;
using Rhino.Mocks;

namespace MixPlanner.Specs
{
    public static class TestMixes
    {
        public static Mix GetRandomMix()
        {
            var mix = GetEmptyMix();
            mix.Add(TestTracks.Get(HarmonicKey.RandomKey()));
            mix.Add(TestTracks.Get(HarmonicKey.RandomKey()));
            mix.Add(TestTracks.Get(HarmonicKey.RandomKey()));
            mix.Add(TestTracks.Get(HarmonicKey.RandomKey()));
            mix.Add(TestTracks.Get(HarmonicKey.RandomKey()));
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