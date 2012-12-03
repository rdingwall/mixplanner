﻿using System;
using System.IO;
using MixPlanner.DomainModel;

namespace MixPlanner.Mp3
{
    public interface ITrackLoader
    {
        Track Load(string filename);
    }

    public class TrackLoader : ITrackLoader
    {
        readonly IId3Reader id3Reader;

        public TrackLoader(IId3Reader id3Reader)
        {
            if (id3Reader == null) throw new ArgumentNullException("id3Reader");
            this.id3Reader = id3Reader;
        }

        public Track Load(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");

            Id3Tag id3Tag;
            if (id3Reader.TryRead(filename, out id3Tag))
                return LoadTrackFromId3Tag(filename, id3Tag);

            return LoadTrackWithoutId3Tags(filename);
        }

        static Track LoadTrackFromId3Tag(string filename, Id3Tag id3Tag)
        {
            var artist = id3Tag.Artist ?? "Unknown Artist";
            var title = id3Tag.Title ?? "Unknown Title";

            HarmonicKey key;
            if (!HarmonicKey.TryParse(id3Tag.InitialKey, out key))
                key = HarmonicKey.Unknown;

            double bpm;
            if (!Double.TryParse(id3Tag.Bpm, out bpm))
                bpm = double.NaN;

            var track = new Track(artist, title, key, filename, bpm)
                            {
                                Label = id3Tag.Publisher ?? "",
                                Genre = id3Tag.Genre ?? "",
                                Year = id3Tag.Year ?? ""
                            };

            return track;
        }

        static Track LoadTrackWithoutId3Tags(string filename)
        {
            var displayName = Path.GetFileNameWithoutExtension(filename);

            return new Track("Unknown Artist", displayName, 
                HarmonicKey.Unknown, filename, float.NaN);
        }
    }
}