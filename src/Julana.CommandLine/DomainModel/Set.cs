using System;
using System.Collections.Generic;
using System.Linq;

namespace Julana.CommandLine.DomainModel
{
    public class Set
    {
        readonly IList<Track> unplayedTracks;
        readonly IList<Track> trackList;

        public Set(IEnumerable<Track> unplayedTracks)
        {
            if (unplayedTracks == null) throw new ArgumentNullException("unplayedTracks");
            this.unplayedTracks = new List<Track>(unplayedTracks);
            trackList = new List<Track>();
        }

        public IEnumerable<Track> UnplayedTracks { get { return unplayedTracks; } }
        public IEnumerable<Track> TrackList { get { return trackList; } }

        public void Play(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");

            trackList.Add(track);
            unplayedTracks.Remove(track);
        }
    }
}