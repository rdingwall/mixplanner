using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.DomainModel;

namespace MixPlanner.Specs
{
    public static class TestMixes
    {
        public static Mix GetRandomMix()
        {
            var mix = new Mix(new Messenger(), new TransitionDetector(Strategies.AllStrategies));
            mix.Add(TestTracks.Get(HarmonicKey.RandomKey()));
            mix.Add(TestTracks.Get(HarmonicKey.RandomKey()));
            mix.Add(TestTracks.Get(HarmonicKey.RandomKey()));
            mix.Add(TestTracks.Get(HarmonicKey.RandomKey()));
            mix.Add(TestTracks.Get(HarmonicKey.RandomKey()));
            return mix;
        }
    }
}