using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.AutoMixing;
using SharpTestsEx;

namespace MixPlanner.Specs.DomainModel.AutoMixing
{
    [Subject(typeof(AutoMixingStrategy))]
    public class AutoMixingStrategySpecs
    {
        public class When_there_was_a_valid_mix
        {
            Establish context =
                () =>
                {
                    testCase = LongestPathAlgorithmTestCases.MixWithMultiplePaths;

                    unknownTracks = new[]
                                             {
                                                 new TestMixItem(isUnknownKeyOrBpm: true),
                                                 new TestMixItem(isUnknownKeyOrBpm: true)
                                             };

                    tracksToMix = testCase.Tracks.Select(t => new TestMixItem(t));

                    mixingContext = new AutoMixingContext(tracksToMix.Concat(unknownTracks));

                    var strategiesFactory = TestMixingStrategies.GetFactory();
                    strategy = new AutoMixingStrategy(strategiesFactory, new EdgeCostCalculator(strategiesFactory));
                };

            Because of = () => result = strategy.AutoMix(mixingContext);

            It should_return_a_successful_result =
                () => result.IsSuccess.ShouldBeTrue();

            It should_separate_the_unmixable_tracks =
                () => result.UnknownTracks.Should().Have.SameValuesAs(unknownTracks);

            It should_find_the_resulting_path =
                () => result.MixedTracks.Select(t => t.ActualKey)
                            .Should().Have.SameSequenceAs(testCase.ExpectedPaths[result.MixedTracks.First().ActualKey]);

            It should_be_a_valid_harmonic_mix =
                () => result.MixedTracks.ShouldAllBeValidTransitions(TestMixingStrategies.PreferredStrategies);

            static IAutoMixingStrategy strategy;
            static AutoMixingContext mixingContext;
            static AutoMixingResult result;
            static IEnumerable<IMixItem> unknownTracks;
            static IEnumerable<IMixItem> tracksToMix;
            static LongestPathAlgorithmTestCase testCase;
        }

        public class When_a_preceeding_and_following_track_were_specified
        {
            Establish context =
                () =>
                {
                    testCase = LongestPathAlgorithmTestCases.MixWithMultiplePaths;

                    unknownTracks = new[]
                                         {
                                             new TestMixItem(isUnknownKeyOrBpm: true),
                                             new TestMixItem(isUnknownKeyOrBpm: true)
                                         };

                    tracksToMix = testCase.Tracks.Select(t => new TestMixItem(t));

                    preceedingTrack = new TestMixItem(HarmonicKey.Key7A);
                    followingTrack = new TestMixItem(HarmonicKey.Key8A);

                    expectedMix = testCase.ExpectedPaths[HarmonicKey.Key7A];

                    mixingContext = new AutoMixingContext(
                        tracksToMix.Concat(unknownTracks), preceedingTrack, followingTrack);

                    var strategiesFactory = TestMixingStrategies.GetFactory();
                    strategy = new AutoMixingStrategy(strategiesFactory, new EdgeCostCalculator(strategiesFactory));
                };

            Because of = () => result = strategy.AutoMix(mixingContext);

            It should_return_a_successful_result =
                () => result.IsSuccess.ShouldBeTrue();

            It should_separate_the_unmixable_tracks =
                () => result.UnknownTracks.Should().Have.SameValuesAs(unknownTracks);

            It should_find_the_resulting_using_the_specified_start_and_end_key =
                () => result.MixedTracks.Select(t => t.ActualKey)
                            .Should().Have.SameSequenceAs(expectedMix);

            It should_be_a_valid_harmonic_mix =
                () => result.MixedTracks.ShouldAllBeValidTransitions(TestMixingStrategies.PreferredStrategies);

            static IAutoMixingStrategy strategy;
            static AutoMixingContext mixingContext;
            static AutoMixingResult result;
            static IEnumerable<TestMixItem> unknownTracks;
            static IEnumerable<TestMixItem> tracksToMix;
            static IEnumerable<HarmonicKey> expectedMix;
            static LongestPathAlgorithmTestCase testCase;
            static TestMixItem preceedingTrack;
            static TestMixItem followingTrack;
        }

        public class When_a_preceeding_and_following_track_were_specified_that_are_outside_the_mix
        {
            Establish context =
                () =>
                {
                    testCase = LongestPathAlgorithmTestCases.MixWithMultiplePaths;

                    unknownTracks = new[]
                                         {
                                             new TestMixItem(isUnknownKeyOrBpm: true),
                                             new TestMixItem(isUnknownKeyOrBpm: true)
                                         };

                    tracksToMix = testCase.Tracks.Select(t => new TestMixItem(t));

                    preceedingTrack = new TestMixItem(HarmonicKey.Key5A);
                    followingTrack = new TestMixItem(HarmonicKey.Key8A);

                    expectedMix = testCase.ExpectedPaths[HarmonicKey.Key6A];

                    mixingContext = new AutoMixingContext(
                        tracksToMix.Concat(unknownTracks), preceedingTrack, followingTrack);

                    var strategiesFactory = TestMixingStrategies.GetFactory();
                    strategy = new AutoMixingStrategy(strategiesFactory, new EdgeCostCalculator(strategiesFactory));
                };

            Because of = () => result = strategy.AutoMix(mixingContext);

            It should_return_a_successful_result =
                () => result.IsSuccess.ShouldBeTrue();

            It should_separate_the_unmixable_tracks =
                () => result.UnknownTracks.Should().Have.SameValuesAs(unknownTracks);

            It should_find_the_resulting_using_the_specified_start_and_end_key =
                () => result.MixedTracks.Select(t => t.ActualKey)
                            .Should().Have.SameSequenceAs(expectedMix);

            It should_be_a_valid_harmonic_mix =
                () => result.MixedTracks.ShouldAllBeValidTransitions(TestMixingStrategies.PreferredStrategies);

            static IAutoMixingStrategy strategy;
            static AutoMixingContext mixingContext;
            static AutoMixingResult result;
            static IEnumerable<TestMixItem> unknownTracks;
            static IEnumerable<TestMixItem> tracksToMix;
            static IEnumerable<HarmonicKey> expectedMix;
            static LongestPathAlgorithmTestCase testCase;
            static TestMixItem preceedingTrack;
            static TestMixItem followingTrack;
        }

        public class When_a_following_track_was_specified_that_was_outside_the_mix
        {
            Establish context =
                () =>
                {
                    testCase = LongestPathAlgorithmTestCases.MixWithMultiplePaths;

                    unknownTracks = new[]
                                         {
                                             new TestMixItem(isUnknownKeyOrBpm: true),
                                             new TestMixItem(isUnknownKeyOrBpm: true)
                                         };

                    tracksToMix = testCase.Tracks.Select(t => new TestMixItem(t));

                    followingTrack = new TestMixItem(HarmonicKey.Key10A);

                    mixingContext = new AutoMixingContext(
                        tracksToMix.Concat(unknownTracks), null, followingTrack);

                    var strategiesFactory = TestMixingStrategies.GetFactory();
                    strategy = new AutoMixingStrategy(strategiesFactory, new EdgeCostCalculator(strategiesFactory));
                };

            Because of = () => result = strategy.AutoMix(mixingContext);

            It should_return_a_successful_result =
                () => result.IsSuccess.ShouldBeTrue();

            It should_separate_the_unmixable_tracks =
                () => result.UnknownTracks.Should().Have.SameValuesAs(unknownTracks);

            It should_find_the_resulting_using_the_specified_end_key =
                () => result.MixedTracks.Select(t => t.ActualKey).Last()
                            .ShouldEqual(HarmonicKey.Key8A);

            It should_be_a_valid_harmonic_mix =
                () => result.MixedTracks.ShouldAllBeValidTransitions(TestMixingStrategies.PreferredStrategies);

            static IAutoMixingStrategy strategy;
            static AutoMixingContext mixingContext;
            static AutoMixingResult result;
            static IEnumerable<TestMixItem> unknownTracks;
            static IEnumerable<TestMixItem> tracksToMix;
            static LongestPathAlgorithmTestCase testCase;
            static TestMixItem followingTrack;
        }

        public class When_the_preceeding_and_following_track_were_the_same_and_required_fallback_strategies
        {
            Establish context =
                () =>
                {
                    testCase = LongestPathAlgorithmTestCases.MixWithMultiplePaths;

                    unknownTracks = new[]
                                         {
                                             new TestMixItem(isUnknownKeyOrBpm: true),
                                             new TestMixItem(isUnknownKeyOrBpm: true)
                                         };

                    tracksToMix = testCase.Tracks.Select(t => new TestMixItem(t));

                    preceedingTrack = new TestMixItem(HarmonicKey.Key12A);
                    followingTrack = new TestMixItem(HarmonicKey.Key12A);

                    mixingContext = new AutoMixingContext(
                        tracksToMix.Concat(unknownTracks), preceedingTrack, followingTrack);

                    var strategiesFactory = TestMixingStrategies.GetFactory();
                    strategy = new AutoMixingStrategy(strategiesFactory, new EdgeCostCalculator(strategiesFactory));
                };

            Because of = () => result = strategy.AutoMix(mixingContext);

            It should_return_a_successful_result =
                () => result.IsSuccess.ShouldBeTrue();

            It should_separate_the_unmixable_tracks =
                () => result.UnknownTracks.Should().Have.SameValuesAs(unknownTracks);

            It should_return_the_correct_number_of_tracks =
                () => result.MixedTracks.Should().Have.SameValuesAs(tracksToMix);

            It should_find_a_mix_starting_with_the_correct_key =
                () => result.MixedTracks.First().ActualKey.ShouldFollowKey(HarmonicKey.Key12A, TestMixingStrategies.AllCompatibleStrategies);

            It should_find_a_mix_ending_with_the_correct_key =
                () => result.MixedTracks.Last().ActualKey.ShouldPreceedKey(HarmonicKey.Key12A, TestMixingStrategies.AllCompatibleStrategies);

            It should_be_a_valid_harmonic_mix =
                () => result.MixedTracks.ShouldAllBeValidTransitions(TestMixingStrategies.AllCompatibleStrategies);

            static IAutoMixingStrategy strategy;
            static AutoMixingContext mixingContext;
            static AutoMixingResult result;
            static IEnumerable<IMixItem> unknownTracks;
            static IEnumerable<IMixItem> tracksToMix;
            static LongestPathAlgorithmTestCase testCase;
            static IMixItem preceedingTrack;
            static IMixItem followingTrack;
        }

        public class When_there_was_no_valid_mix
        {
            Establish context =
                () =>
                {
                    testCase = LongestPathAlgorithmTestCases.MixWithNoPaths;

                    unknownTracks = new[]
                                             {
                                                 new TestMixItem(isUnknownKeyOrBpm: true),
                                                 new TestMixItem(isUnknownKeyOrBpm: true)
                                             };

                    tracksToMix = testCase.Tracks.Select(t => new TestMixItem(t));

                    mixingContext = new AutoMixingContext(tracksToMix.Concat(unknownTracks));

                    var strategiesFactory = TestMixingStrategies.GetFactory();
                    strategy = new AutoMixingStrategy(strategiesFactory, new EdgeCostCalculator(strategiesFactory));
                };

            Because of = () => result = strategy.AutoMix(mixingContext);

            It should_return_a_failed_result =
                () => result.IsSuccess.ShouldBeFalse();

            It should_not_separate_the_unmixable_tracks =
                () => result.UnknownTracks.Should().Be.Empty();

            It should_return_the_tracks_as_is =
                () => result.MixedTracks.Should().Have.SameSequenceAs(mixingContext.TracksToMix);

            static IAutoMixingStrategy strategy;
            static AutoMixingContext mixingContext;
            static AutoMixingResult result;
            static IEnumerable<IMixItem> unknownTracks;
            static IEnumerable<IMixItem> tracksToMix;
            static LongestPathAlgorithmTestCase testCase;
        }

        public class TestMixItem : IMixItem, IEquatable<TestMixItem>
        {
            public PlaybackSpeed PlaybackSpeed { get; private set; }
            public Track Track { get; private set; }
            public Transition Transition { get; set; }
            public HarmonicKey ActualKey { get; set; }
            public bool IsUnknownKeyOrBpm { get; set; }
            public double ActualBpm { get { return 128; } }

            public TestMixItem(bool isUnknownKeyOrBpm)
            {
                IsUnknownKeyOrBpm = isUnknownKeyOrBpm;
            }

            public TestMixItem(HarmonicKey key)
            {
                ActualKey = key;
            }

            public TestMixItem(PlaybackSpeed playbackSpeed)
            {
                ActualKey = playbackSpeed.ActualKey;
            }

            public override string ToString()
            {
                return ActualKey.ToString();
            }

            public bool Equals(TestMixItem other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Equals(ActualKey, other.ActualKey) && IsUnknownKeyOrBpm.Equals(other.IsUnknownKeyOrBpm);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((TestMixItem)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (ActualKey.GetHashCode() * 397) ^ IsUnknownKeyOrBpm.GetHashCode();
                }
            }
        }
    }
}