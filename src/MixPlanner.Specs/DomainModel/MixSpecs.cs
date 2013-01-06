using GalaSoft.MvvmLight.Messaging;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.Events;

namespace MixPlanner.Specs.DomainModel
{
    [Subject(typeof(Mix)), Ignore("A bit brittle, will wait")]
    public class MixSpecs
    {
        public class When_adding_the_opening_track_for_an_empty_mix
        {
            Establish context =
                () =>
                    {
                        track = TestTracks.Get(HarmonicKey.Key1A);
                        messenger = new DispatcherMessenger(new Messenger());
                        messenger.Register<TrackAddedToMixEvent>(new object(), e => ev = e);
                        mix = new Mix(messenger, new ActualTransitionDetector(TestMixingStrategies.AllStrategies));
                    };

            Because of = () => mix.Insert(track, 0);

            It should_raise_an_event = () => ev.ShouldNotBeNull();

            It should_notify_which_track_was_added =
                () => ev.Item.Track.ShouldEqual(track);

            It should_notify_the_index_the_track_was_inserted_at =
                () => ev.InsertIndex.ShouldEqual(0);

            static TrackAddedToMixEvent ev;
            static Track track;
            static Mix mix;
            static IDispatcherMessenger messenger;
        }
    }
}