using System.Collections.Generic;
using MixPlanner.DomainModel;
using SharpTestsEx;

namespace MixPlanner.Specs
{
    public class MixItemAssertComparer : IEqualityComparer<IMixItem>
    {
        static readonly TrackAssertComparer TrackComparer = new TrackAssertComparer();

        public bool Equals(IMixItem x, IMixItem y)
        {
            x.ActualBpm.Should().Be(y.ActualBpm);
            x.ActualKey.Should().Be(y.ActualKey);
            x.IsUnknownKeyOrBpm.Should().Be(y.IsUnknownKeyOrBpm);
            x.Transition.Strategy.Should().Be(y.Transition.Strategy);
            x.Transition.FromKey.Should().Be(y.Transition.FromKey);
            x.Transition.ToKey.Should().Be(y.Transition.ToKey);
            x.Transition.IncreaseRequired.Should().Be(y.Transition.IncreaseRequired);
            x.Transition.IsIntro.Should().Be(y.Transition.IsIntro);
            x.Transition.IsOutro.Should().Be(y.Transition.IsOutro);

            x.Track.Should().Be(y.Track);
            x.Transition.Should().Be(y.Transition);

            return TrackComparer.Equals(x.Track, y.Track);
        }

        public int GetHashCode(IMixItem obj)
        {
            return obj.GetHashCode();
        }
    }

    public class TrackAssertComparer : IEqualityComparer<Track>
    {
        public bool Equals(Track x, Track y)
        {
            x.Id.Should().Be(y.Id);
            x.OriginalBpm.Should().Be(y.OriginalBpm);
            x.OriginalKey.Should().Be(y.OriginalKey);
            x.Title.Should().Be(y.Title);
            x.Year.Should().Be(y.Year);
            x.Label.Should().Be(y.Label);
            x.Genre.Should().Be(y.Genre);
            x.Filename.Should().Be(y.Filename);
            x.Duration.Should().Be(y.Duration);
            x.Artist.Should().Be(y.Artist);
            return true;
        }

        public int GetHashCode(Track obj)
        {
            return obj.GetHashCode();
        }
    }
}