﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using MixPlanner.Converters;
using MixPlanner.DomainModel;

namespace MixPlanner.Mp3
{
    public interface ITrackLoader
    {
        Task<Track> LoadAsync(string filename);
    }

    public class TrackLoader : ITrackLoader
    {
        readonly IId3Reader id3Reader;
        readonly IId3TagCleanupFactory cleanupFactory;
        readonly ITrackImageResizer imageResizer;
        readonly IEnumerable<IValueConverter> notationConverters;

        public TrackLoader(
            IId3Reader id3Reader, 
            IId3TagCleanupFactory cleanupFactory,
            ITrackImageResizer imageResizer, 
            IHarmonicKeyConverterFactory converterFactory)
        {
            if (id3Reader == null) throw new ArgumentNullException("id3Reader");
            if (cleanupFactory == null) throw new ArgumentNullException("cleanupFactory");
            if (imageResizer == null) throw new ArgumentNullException("imageResizer");
            if (converterFactory == null) throw new ArgumentNullException("converterFactory");
            this.id3Reader = id3Reader;
            this.cleanupFactory = cleanupFactory;
            this.imageResizer = imageResizer;
            notationConverters = converterFactory.GetAllConverters();
        }

        public async Task<Track> LoadAsync(string filename)
        {
            return await Task.Run(() => Load(filename));
        }

        Track Load(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");

            Id3Tag id3Tag;
            if (id3Reader.TryRead(filename, out id3Tag))
                return LoadTrackFromId3Tag(filename, id3Tag);

            return LoadTrackWithoutId3Tags(filename);
        }

        Track LoadTrackFromId3Tag(string filename, Id3Tag id3Tag)
        {
            foreach (var cleanup in cleanupFactory.GetCleanups())
                cleanup.Clean(id3Tag);

            HarmonicKey key = ParseHarmonicKey(id3Tag.InitialKey);

            double bpm;
            if (!Double.TryParse(id3Tag.Bpm, out bpm))
                bpm = double.NaN;

            TrackImageData images = 
                id3Tag.ImageData != null ? imageResizer.Process(id3Tag.ImageData) : null;

            var track = new Track(id3Tag.Artist, id3Tag.Title, key, filename, bpm)
                            {
                                Label = id3Tag.Publisher ?? "",
                                Genre = id3Tag.Genre ?? "",
                                Year = id3Tag.Year ?? "",
                                Images = images
                            };

            return track;
        }

        static Track LoadTrackWithoutId3Tags(string filename)
        {
            var displayName = Path.GetFileNameWithoutExtension(filename);

            return new Track(TrackDefaults.UnknownArtist, displayName, 
                HarmonicKey.Unknown, filename, float.NaN);
        }

        HarmonicKey ParseHarmonicKey(string str)
        {
            if (String.IsNullOrWhiteSpace(str))
                return HarmonicKey.Unknown;

            var key = notationConverters
                .Select(c => c.ConvertBack(str, typeof(HarmonicKey), null, null))
                .OfType<HarmonicKey>()
                .FirstOrDefault(k => k != HarmonicKey.Unknown);

            return key ?? HarmonicKey.Unknown;
        }
    }
}