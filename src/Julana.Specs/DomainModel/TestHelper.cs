using System.Collections.Generic;
using Julana.CommandLine.DomainModel;

namespace Julana.Specs.DomainModel
{
    public static class TestHelper
    {
        public static IEnumerable<Track> GetDummyTracks()
        {
            return new List<Track>
                       {
                           new Track("A - A", Key.RandomKey()),
                           new Track("B - B", Key.RandomKey()),
                           new Track("C - C", Key.RandomKey()),
                           new Track("D - D", Key.RandomKey()),
                           new Track("E - E", Key.RandomKey()),
                           new Track("F - F", Key.RandomKey()),
                           new Track("G - G", Key.RandomKey()),
                           new Track("H - H", Key.RandomKey()),
                           new Track("I - I", Key.RandomKey()),
                           new Track("J - J", Key.RandomKey()),
                           new Track("K - K", Key.RandomKey())
                       };
        }
    }
}