using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.DomainModel;
using MixPlanner.DomainModel.AutoMixing;
using SharpTestsEx;

namespace MixPlanner.Specs.DomainModel.AutoMixing
{
    [Subject(typeof(AutoMixingStrategy<>))]
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
                                                 new TestTrack(isUnknownKeyOrBpm: true),
                                                 new TestTrack(isUnknownKeyOrBpm: true)
                                             };

                    tracksToMix = testCase.Tracks.Select(t => new TestTrack(t));

                    mixingContext = new AutoMixingContext<TestTrack>(tracksToMix.Concat(unknownTracks));

                    strategy = new AutoMixingStrategy<TestTrack>(TestMixingStrategies.GetFactory());
                };

            Because of = () => result = strategy.AutoMix(mixingContext);

            It should_return_a_successful_result =
                () => result.IsSuccess.ShouldBeTrue();

            It should_separate_the_unmixable_tracks =
                () => result.UnknownTracks.Should().Have.SameValuesAs(unknownTracks);

            It should_find_the_resulting_path =
                () => result.MixedTracks.Select(t => t.PlaybackSpeed.ActualKey)
                            .Should().Have.SameSequenceAs(testCase.ExpectedPaths[result.MixedTracks.First().PlaybackSpeed.ActualKey]);

            static IAutoMixingStrategy<TestTrack> strategy;
            static AutoMixingContext<TestTrack> mixingContext;
            static AutoMixingResult<TestTrack> result;
            static IEnumerable<TestTrack> unknownTracks;
            static IEnumerable<TestTrack> tracksToMix;
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
                                             new TestTrack(isUnknownKeyOrBpm: true),
                                             new TestTrack(isUnknownKeyOrBpm: true)
                                         };

                    tracksToMix = testCase.Tracks.Select(t => new TestTrack(t));

                    preceedingTrack = new TestTrack(HarmonicKey.Key7A);
                    followingTrack = new TestTrack(HarmonicKey.Key8A);

                    expectedMix = testCase.ExpectedPaths[HarmonicKey.Key7A];

                    mixingContext = new AutoMixingContext<TestTrack>(
                        tracksToMix.Concat(unknownTracks), preceedingTrack, followingTrack);

                    strategy = new AutoMixingStrategy<TestTrack>(TestMixingStrategies.GetFactory());
                };

            Because of = () => result = strategy.AutoMix(mixingContext);

            It should_return_a_successful_result =
                () => result.IsSuccess.ShouldBeTrue();

            It should_separate_the_unmixable_tracks =
                () => result.UnknownTracks.Should().Have.SameValuesAs(unknownTracks);

            It should_find_the_resulting_using_the_specified_start_and_end_key =
                () => result.MixedTracks.Select(t => t.PlaybackSpeed.ActualKey)
                            .Should().Have.SameSequenceAs(expectedMix);

            static IAutoMixingStrategy<TestTrack> strategy;
            static AutoMixingContext<TestTrack> mixingContext;
            static AutoMixingResult<TestTrack> result;
            static IEnumerable<TestTrack> unknownTracks;
            static IEnumerable<TestTrack> tracksToMix;
            static IEnumerable<HarmonicKey> expectedMix;
            static LongestPathAlgorithmTestCase testCase;
            static TestTrack preceedingTrack;
            static TestTrack followingTrack;
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
                                             new TestTrack(isUnknownKeyOrBpm: true),
                                             new TestTrack(isUnknownKeyOrBpm: true)
                                         };

                    tracksToMix = testCase.Tracks.Select(t => new TestTrack(t));

                    preceedingTrack = new TestTrack(HarmonicKey.Key12A);
                    followingTrack = new TestTrack(HarmonicKey.Key12A);

                    expectedMix = testCase.ExpectedPaths[HarmonicKey.Key7A];

                    mixingContext = new AutoMixingContext<TestTrack>(
                        tracksToMix.Concat(unknownTracks), preceedingTrack, followingTrack);

                    strategy = new AutoMixingStrategy<TestTrack>(TestMixingStrategies.GetFactory());
                };

            Because of = () => result = strategy.AutoMix(mixingContext);

            It should_return_a_successful_result =
                () => result.IsSuccess.ShouldBeTrue();

            It should_separate_the_unmixable_tracks =
                () => result.UnknownTracks.Should().Have.SameValuesAs(unknownTracks);

            It should_find_the_resulting_using_the_specified_start_and_end_key =
                () => result.MixedTracks.Select(t => t.PlaybackSpeed.ActualKey)
                            .Should().Have.SameSequenceAs(expectedMix);

            static IAutoMixingStrategy<TestTrack> strategy;
            static AutoMixingContext<TestTrack> mixingContext;
            static AutoMixingResult<TestTrack> result;
            static IEnumerable<TestTrack> unknownTracks;
            static IEnumerable<TestTrack> tracksToMix;
            static IEnumerable<HarmonicKey> expectedMix;
            static LongestPathAlgorithmTestCase testCase;
            static TestTrack preceedingTrack;
            static TestTrack followingTrack;
        }

        public class When_there_was_no_valid_mix
        {
            Establish context =
                () =>
                {
                    testCase = LongestPathAlgorithmTestCases.MixWithNoPaths;

                    unknownTracks = new[]
                                             {
                                                 new TestTrack(isUnknownKeyOrBpm: true),
                                                 new TestTrack(isUnknownKeyOrBpm: true)
                                             };

                    tracksToMix = testCase.Tracks.Select(t => new TestTrack(t));

                    mixingContext = new AutoMixingContext<TestTrack>(tracksToMix.Concat(unknownTracks));

                    strategy = new AutoMixingStrategy<TestTrack>(TestMixingStrategies.GetFactory());
                };

            Because of = () => result = strategy.AutoMix(mixingContext);

            It should_return_a_failed_result =
                () => result.IsSuccess.ShouldBeFalse();

            It should_not_separate_the_unmixable_tracks =
                () => result.UnknownTracks.Should().Be.Empty();

            It should_return_the_tracks_as_is =
                () => result.MixedTracks.Should().Have.SameSequenceAs(mixingContext.TracksToMix);

            static IAutoMixingStrategy<TestTrack> strategy;
            static AutoMixingContext<TestTrack> mixingContext;
            static AutoMixingResult<TestTrack> result;
            static IEnumerable<TestTrack> unknownTracks;
            static IEnumerable<TestTrack> tracksToMix;
            static LongestPathAlgorithmTestCase testCase;
        }

        public class TestTrack : IAutoMixable, IEquatable<TestTrack>
        {
            public PlaybackSpeed PlaybackSpeed { get; set; }
            public bool IsUnknownKeyOrBpm { get; set; }

            public TestTrack(bool isUnknownKeyOrBpm)
            {
                IsUnknownKeyOrBpm = isUnknownKeyOrBpm;
            }

            public TestTrack(PlaybackSpeed playbackSpeed)
            {
                PlaybackSpeed = playbackSpeed;
            }

            public TestTrack(HarmonicKey key)
            {
                PlaybackSpeed = new PlaybackSpeed(key, 128);
            }

            public override string ToString()
            {
                if (PlaybackSpeed == null)
                    return "unknown";

                return PlaybackSpeed.ActualKey.ToString();
            }

            public bool Equals(TestTrack other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Equals(PlaybackSpeed, other.PlaybackSpeed) && IsUnknownKeyOrBpm.Equals(other.IsUnknownKeyOrBpm);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((TestTrack)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((PlaybackSpeed != null ? PlaybackSpeed.GetHashCode() : 0) * 397) ^ IsUnknownKeyOrBpm.GetHashCode();
                }
            }
        }
    }
}