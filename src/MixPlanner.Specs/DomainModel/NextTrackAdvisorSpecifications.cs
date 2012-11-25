using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.CommandLine.DomainModel;
using MixPlanner.CommandLine.DomainModel.MixingStrategies;

namespace MixPlanner.Specs.DomainModel
{
    [Subject(typeof(NextTrackAdvisor))]
    public class NextTrackAdvisorSpecifications
    {
        public class when_constructing
        {
            static Type[] expectedOrderOfStrategies;

            Establish context =
                () =>
                    {
                        expectedOrderOfStrategies = new[]
                                                        {
                                                            typeof (TwoSemitoneEnergyBoost),
                                                            typeof (SameKey),
                                                            typeof (OneSemitoneEnergyBoost),
                                                            typeof (IncrementOne),
                                                            typeof (SwitchToMajorScale),
                                                            typeof (SwitchToMinorScale)
                                                        };
                    };

            Because of = () => actualOrder = new NextTrackAdvisor().PreferredStrategies.Select(s => s.GetType());

            It should_prefer_the_best_strategies_first =
                () => actualOrder.ShouldEqual(expectedOrderOfStrategies);

            static IEnumerable<Type> actualOrder; 
        }

        public class DummyMixingStrategy : IMixingStrategy
        {
            public Track Track;

            public DummyMixingStrategy(Track track)
            {
                if (track == null) throw new ArgumentNullException("track");
                Track = track;
            }

            public IEnumerable<Track> NextSuggestedTracks(Track currentTrack, IEnumerable<Track> unplayedTracks)
            {
                yield return Track;
            }
        }

        public class when_advising_the_next_track
        {
            static IDictionary<Track, IMixingStrategy> suggestions;

            Establish context =
                () =>
                    {
                        allTracks = TestMixes.GetRandomMix();
                        currentTrack = TestTracks.Get(Key.RandomKey());

                        strategyA = new DummyMixingStrategy(allTracks.Last());
                        strategyB = new DummyMixingStrategy(allTracks.First());

                        advisor = new NextTrackAdvisor();

                        advisor.PreferredStrategies.Clear();
                        advisor.PreferredStrategies.Add(strategyA);
                        advisor.PreferredStrategies.Add(strategyB);
                    };

            Because of = () => suggestions = advisor.GetSuggestionsForNextTrack(currentTrack, allTracks);

            static NextTrackAdvisor advisor;
            static IEnumerable<Track> allTracks;

            It should_suggest_tracks_from_the_preferred_strategies_first =
                () => suggestions.ShouldEqual(new Dictionary<Track, IMixingStrategy>
                                                  {
                                                      {strategyA.Track, strategyA},
                                                      {strategyB.Track, strategyB}
                                                  });

            static DummyMixingStrategy strategyA;
            static DummyMixingStrategy strategyB;
            static Track currentTrack;
        }
    }
}