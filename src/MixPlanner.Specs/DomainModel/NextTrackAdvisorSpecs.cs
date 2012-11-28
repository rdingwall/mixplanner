using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MixPlanner.DomainModel;

namespace MixPlanner.Specs.DomainModel
{
    [Subject(typeof(NextTrackAdvisor))]
    public class NextTrackAdvisorSpecs
    {
        public class DummyMixingStrategy : IMixingStrategy
        {
            public Track Track;

            public DummyMixingStrategy(Track track)
            {
                if (track == null) throw new ArgumentNullException("track");
                Track = track;
            }

            public bool IsCompatible(Track firstTrack, Track secondTrack)
            {
                return secondTrack.Equals(Track);
            }

            public IEnumerable<Track> NextSuggestedTracks(Track currentTrack, IEnumerable<Track> unplayedTracks)
            {
                yield return Track;
            }

            public string Description { get; private set; }
        }

        public class when_advising_the_next_track
        {
            static IDictionary<Track, IMixingStrategy> suggestions;

            Establish context =
                () =>
                    {
                        allTracks = TestMixes.GetRandomMix();
                        currentTrack = TestTracks.Get(HarmonicKey.RandomKey());

                        strategyA = new DummyMixingStrategy(allTracks.Last());
                        strategyB = new DummyMixingStrategy(allTracks.First());

                        advisor = new NextTrackAdvisor(new[] { strategyA, strategyB});
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