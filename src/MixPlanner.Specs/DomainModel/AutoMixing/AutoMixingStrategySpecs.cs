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
                         var @case = LongestPathAlgorithmTestCases.MixWithMultiplePaths;

                         unknownTracks = new[]
                                             {
                                                 new TestTrack {IsUnknownKeyOrBpm = true},
                                                 new TestTrack {IsUnknownKeyOrBpm = true}
                                             };

                         tracksToMix = @case.Tracks.Select(t => new TestTrack {PlaybackSpeed = t});

                         expectedMix = @case.ExpectedPaths.First().Value;

                         mixingContext = new AutoMixingContext<TestTrack>(tracksToMix.Concat(unknownTracks));

                         strategy = new AutoMixingStrategy<TestTrack>(TestMixingStrategies.GetFactory());
                     };

             Because of = () => result = strategy.AutoMix(mixingContext);

             It should_separate_the_unmixable_tracks =
                 () => result.UnknownTracks.Should().Have.SameValuesAs(unknownTracks);

             It should_find_the_resulting_path =
                 () => result.MixedTracks.Select(t => t.PlaybackSpeed.ActualKey)
                             .Should().Have.SameSequenceAs(expectedMix);

             static IAutoMixingStrategy<TestTrack> strategy;
             static AutoMixingContext<TestTrack> mixingContext;
             static AutoMixingResult<TestTrack> result;
             static IEnumerable<TestTrack> unknownTracks;
             static IEnumerable<TestTrack> tracksToMix;
             static IEnumerable<HarmonicKey> expectedMix;
         }

         public class TestTrack : IAutoMixable, IEquatable<TestTrack>
         {
             public PlaybackSpeed PlaybackSpeed { get; set; }
             public bool IsUnknownKeyOrBpm { get; set; }

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
                 return Equals((TestTrack) obj);
             }

             public override int GetHashCode()
             {
                 unchecked
                 {
                     return ((PlaybackSpeed != null ? PlaybackSpeed.GetHashCode() : 0)*397) ^ IsUnknownKeyOrBpm.GetHashCode();
                 }
             }
         }
    }
}