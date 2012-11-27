﻿using System;
using MixPlanner.DomainModel;

namespace MixPlanner.ViewModels
{
    public class LibraryItemViewModel
    {
        public LibraryItemViewModel(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            Track = track;
            Artist = track.Artist;
            Title = track.Title;
            Genre = track.Genre;
            Bpm = track.Bpm;
            Year = track.Year;
            Label = track.Label;
            Filename = track.File.FullName;
            Key = track.Key.ToString();
        }

        public string Filename { get; set; }

        public string Artist { get; set; }
        public string Title { get; set; }

        public string Key { get; set; }

        public string Genre { get; set; }
        public string Label { get; set; }
        public string Year { get; set; }
        public int? Bpm { get; set; }

        public Track Track { get; set; }
    }
}