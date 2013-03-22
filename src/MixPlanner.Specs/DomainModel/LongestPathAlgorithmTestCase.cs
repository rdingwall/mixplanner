using System;
using System.Collections.Generic;
using System.Linq;
using MixPlanner.DomainModel;

namespace MixPlanner.Specs.DomainModel
{
    public class LongestPathAlgorithmTestCase
    {
        static readonly Random Random = new Random();

        public LongestPathAlgorithmTestCase(IEnumerable<HarmonicKey> harmonicKeys)
        {
            ExpectedPaths = new Dictionary<HarmonicKey, IEnumerable<HarmonicKey>>();
            Keys = harmonicKeys;
            Tracks = Keys
                //.Concat(TestMixes.GetRandomMix(1000).Items.Select(i => i.PlaybackSpeed.ActualKey))
                .Distinct()
                .OrderBy(_ => Random.Next())
                .Select(k => new PlaybackSpeed(k, 128))
                .ToList();
        }

        public IEnumerable<PlaybackSpeed> Tracks { get; private set; }

        public void PrintTracks()
        {
            Console.WriteLine(String.Join(", ", Tracks.Select(k => k.ActualKey).OrderBy(k => k)));
        }

        public IEnumerable<HarmonicKey> Keys { get; private set; } 

        public IDictionary<HarmonicKey, IEnumerable<HarmonicKey>> ExpectedPaths { get; private set; } 
    }
}