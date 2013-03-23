using System;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.Events;

namespace MixPlanner.Specs.DomainModel
{
    [Subject(typeof(Mix))]
    public class MixSpecs
    {
        public class When_adding_the_opening_track_for_an_empty_mix
        {
            Establish context =
                () =>
                    {
                        track = TestTracks.GetRandomTrack(HarmonicKey.Key1A);
                        messenger = new DispatcherMessenger(new Messenger());
                        messenger.Register<TrackAddedToMixEvent>(new object(), e => ev = e);
                        mix = new Mix(messenger, 
                            new ActualTransitionDetector(TestMixingStrategies.AllStrategies),
                            new DummyPlaybackSpeedAdjuster());
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

        public class When_calculating_the_average_bpm_of_the_mix
        {
            Establish context =
                () =>
                {
                    var messenger = new DispatcherMessenger(new Messenger());
                    mix = new Mix(messenger, new ActualTransitionDetector(TestMixingStrategies.AllStrategies),
                                      new DummyPlaybackSpeedAdjuster());
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key1A, 100));
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key1A, 200));
                    mix.Add(TestTracks.GetRandomTrack(HarmonicKey.Key1A, Double.NaN));
                };

            Because of = () => averageBpm = mix.CalculateAverageActualBpm();

            It should_correctly_calculate_the_average_bpm_ignoring_unknown_tracks =
                () => averageBpm.ShouldBeCloseTo(150, 0.001);

            static Mix mix;
            static double averageBpm;
        }

        public class When_auto_adjusting_all_the_tracks_playback_speeds_to_match_the_average_bpm
        {
            Establish context =
                () =>
                {
                    first = TestTracks.GetRandomTrack(128);
                    second = TestTracks.GetRandomTrack(132);
                    third = TestTracks.GetRandomTrack(138);
                    fourth = TestTracks.GetRandomTrack(Double.NaN);

                    var messenger = new DispatcherMessenger(new Messenger());
                    mix = new Mix(messenger, new ActualTransitionDetector(TestMixingStrategies.AllStrategies),
                                      new LimitingPlaybackSpeedAdjuster());
                    mix.Add(first);
                    mix.Add(second);
                    mix.Add(third);
                    mix.Add(fourth);
                };

            Because of = () => mix.AutoAdjustBpms();

            It should_auto_adjust_the_first_track =
                () => mix.GetMixItem(first).PlaybackSpeed.Adjustment.ShouldBeCloseTo(0.03);

            It should_auto_adjust_the_second_track =
                () => mix.GetMixItem(second).PlaybackSpeed.Adjustment.ShouldBeCloseTo(0);

            It should_auto_adjust_the_third_track =
                () => mix.GetMixItem(third).PlaybackSpeed.Adjustment.ShouldBeCloseTo(-0.03);

            It should_not_adjust_the_last_track =
                () => mix.GetMixItem(fourth).PlaybackSpeed.Adjustment.ShouldBeCloseTo(0);

            static Track first;
            static Track second;
            static Track third;
            static Track fourth;
            static IMix mix;
        }

        public class When_moving_a_track_to_the_end
        {
            Establish context = () =>
                                    {
                                        mix = TestMixes.GetRandomMix();
                                        itemToMove = mix.First();
                                    };

            Because of = () => mix.MoveToEnd(itemToMove);

            It should_be_moved_to_the_end =
                () => mix.Last().ShouldEqual(itemToMove);

            static IMix mix;
            static IMixItem itemToMove;
        }
    }
}