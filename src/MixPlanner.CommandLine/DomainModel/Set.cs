using System;
using System.Collections.Generic;
using System.Linq;
using MixPlanner.CommandLine.DomainModel.MixingStrategies;

namespace MixPlanner.CommandLine.DomainModel
{
    public class Set
    {
        readonly INextTrackAdvisor advisor;
        readonly IList<Track> unplayedTracks;
        readonly IList<Track> trackList;

        public Set(IEnumerable<Track> unplayedTracks, INextTrackAdvisor advisor)
        {
            this.advisor = advisor;
            if (unplayedTracks == null) throw new ArgumentNullException("unplayedTracks");
            if (advisor == null) throw new ArgumentNullException("advisor");
            this.unplayedTracks = new List<Track>(unplayedTracks.Distinct());
            trackList = new List<Track>();
        }

        public IEnumerable<Track> UnplayedTracks { get { return unplayedTracks; } }
        public IEnumerable<Track> TrackList { get { return trackList; } }
        public Track CurrentTrack { get { return TrackList.LastOrDefault();  } }

        public void Play(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");

            trackList.Add(track);
            unplayedTracks.Remove(track);
        }

        public IDictionary<Track, IMixingStrategy> NextTrackSuggestions()
        {
            if (CurrentTrack == null)
                return OpeningTrackSuggestions();

            return advisor.GetSuggestionsForNextTrack(trackList.Last(), unplayedTracks);
        }

        Dictionary<Track, IMixingStrategy> OpeningTrackSuggestions()
        {
            return UnplayedTracks.ToDictionary(k => k, v => (IMixingStrategy)new OpeningTrack());
        }
    }
}