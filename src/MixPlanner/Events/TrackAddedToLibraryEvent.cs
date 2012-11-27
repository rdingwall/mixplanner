﻿using System;
using MixPlanner.DomainModel;

namespace MixPlanner.Events
{
    public class TrackAddedToLibraryEvent
    {
        public TrackAddedToLibraryEvent(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            Track = track;
        }

        public Track Track { get; private set; }
    }
}