using System;
using MixPlanner.CommandLine.DomainModel;

namespace MixPlanner.Specs
{
    public static class TestTracks
    {
        public static Track Get(Key key)
        {
            return new Track(key.ToString(), key, string.Format("{0:N}.mp3", Guid.NewGuid()));
        }
    }
}