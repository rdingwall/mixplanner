using System;
using System.Collections.Generic;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class TrackLibraryLoadedEvent
    {
        public TrackLibraryLoadedEvent(IEnumerable<Track> tracks)
        {
            if (tracks == null) throw new ArgumentNullException("tracks");
            Tracks = tracks;
        }

        public IEnumerable<Track> Tracks { get; private set; }
    }
}