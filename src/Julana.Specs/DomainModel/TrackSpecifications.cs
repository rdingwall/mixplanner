using Julana.CommandLine.DomainModel;
using Machine.Specifications;

namespace Julana.Specs.DomainModel
{
    [Subject(typeof(Track))]
    public class TrackSpecifications
    {
        public class when_comparing_two_tracks_with_the_same_name_and_key
        {
            It should_consider_them_equal =
                () => new Track("A", Key.Key9A).ShouldEqual(new Track("A", Key.Key9A));
        }

        public class when_comparing_two_tracks_with_different_keys
        {
            It should_not_consider_them_equal =
                () => new Track("A", Key.Key9A).ShouldNotEqual(new Track("A", Key.Key6B));
        }

        public class when_comparing_two_tracks_with_different_names
        {
            It should_not_consider_them_equal =
                () => new Track("A", Key.Key9A).ShouldNotEqual(new Track("Z", Key.Key9A));
        }
    }
}