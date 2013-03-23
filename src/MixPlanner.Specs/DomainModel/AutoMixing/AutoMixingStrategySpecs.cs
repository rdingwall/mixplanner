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

                    strategy = new AutoMixingStrategy(TestMixingStrategies.GetFactory());
                };

            Because of = () => result = strategy.AutoMix(mixingContext);

            It should_return_a_successful_result =
                () => result.IsSuccess.ShouldBeTrue();

            It should_separate_the_unmixable_tracks =
                () => result.UnknownTracks.Should().Have.SameValuesAs(unknownTracks);

            It should_find_the_resulting_path =
                () => result.MixedTracks.Select(t => t.ActualKey)
                            .Should().Have.SameSequenceAs(testCase.ExpectedPaths[result.MixedTracks.First().ActualKey]);

            static IAutoMixingStrategy strategy;
            static AutoMixingContext mixingContext;
            static AutoMixingResult result;
            static IEnumerable<IMixItem> unknownTracks;
            static IEnumerable<IMixItem> tracksToMix;
            static LongestPathAlgorithmTestCase testCase;
        }

        [Ignore("temp - need to rethink this")]
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

                    strategy = new AutoMixingStrategy(TestMixingStrategies.GetFactory());
                };

            Because of = () => result = strategy.AutoMix(mixingContext);

            It should_return_a_successful_result =
                () => result.IsSuccess.ShouldBeTrue();

            It should_separate_the_unmixable_tracks =
                () => result.UnknownTracks.Should().Have.SameValuesAs(unknownTracks);

            It should_find_the_resulting_using_the_specified_start_and_end_key =
                () => result.MixedTracks.Select(t => t.ActualKey)
                            .Should().Have.SameSequenceAs(expectedMix);

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

        [Ignore("temp - need to rethink this")]
        public class When_a_preceeding_and_following_track_were_specified_but_no_such_paths_were_found
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

                    expectedMix = testCase.ExpectedPaths[HarmonicKey.Key7A];

                    mixingContext = new AutoMixingContext(
                        tracksToMix.Concat(unknownTracks), preceedingTrack, followingTrack);

                    strategy = new AutoMixingStrategy(TestMixingStrategies.GetFactory());
                };

            Because of = () => result = strategy.AutoMix(mixingContext);

            It should_return_a_successful_result =
                () => result.IsSuccess.ShouldBeTrue();

            It should_separate_the_unmixable_tracks =
                () => result.UnknownTracks.Should().Have.SameValuesAs(unknownTracks);

            It should_find_the_resulting_using_the_specified_start_and_end_key =
                () => result.MixedTracks.Select(t => t.ActualKey)
                            .Should().Have.SameSequenceAs(expectedMix);

            static IAutoMixingStrategy strategy;
            static AutoMixingContext mixingContext;
            static AutoMixingResult result;
            static IEnumerable<IMixItem> unknownTracks;
            static IEnumerable<IMixItem> tracksToMix;
            static IEnumerable<HarmonicKey> expectedMix;
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

                    strategy = new AutoMixingStrategy(TestMixingStrategies.GetFactory());
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
                if (ActualKey == null)
                    return "unknown";

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
                    return ((ActualKey != null ? ActualKey.GetHashCode() : 0) * 397) ^ IsUnknownKeyOrBpm.GetHashCode();
                }
            }
        }
    }
}