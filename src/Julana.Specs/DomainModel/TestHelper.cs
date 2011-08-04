using System.Collections.Generic;
using Julana.CommandLine.DomainModel;

namespace Julana.Specs.DomainModel
{
    public static class TestHelper
    {
        public static IEnumerable<Track> GetDummyTracks()
        {
            yield return new Track("A - A", Key.RandomKey());
            yield return new Track("B - B", Key.RandomKey());
            yield return new Track("C - C", Key.RandomKey());
            yield return new Track("D - D", Key.RandomKey());
            yield return new Track("E - E", Key.RandomKey());
            yield return new Track("F - F", Key.RandomKey());
            yield return new Track("G - G", Key.RandomKey());
            yield return new Track("H - H", Key.RandomKey());
            yield return new Track("I - I", Key.RandomKey());
            yield return new Track("J - J", Key.RandomKey());
            yield return new Track("K - K", Key.RandomKey());
        }
    }
}