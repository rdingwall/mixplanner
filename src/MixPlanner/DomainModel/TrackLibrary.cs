using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.Events;
using MixPlanner.Mp3;
using MixPlanner.Storage;
using MoreLinq;

namespace MixPlanner.DomainModel
{
    public class TrackLibrary : ITrackLibrary
    {
        const StringComparison Comparison = StringComparison.CurrentCultureIgnoreCase;
        readonly ITrackLoader loader;
        readonly IMessenger messenger;
        readonly ILibraryStorage storage;

        public TrackLibrary(ITrackLoader loader, IMessenger messenger, ILibraryStorage storage)
        {
            if (loader == null) throw new ArgumentNullException("loader");
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (storage == null) throw new ArgumentNullException("storage");
            this.loader = loader;
            this.messenger = messenger;
            this.storage = storage;
        }

        public IEnumerable<Track> Import(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");

            if (IsDirectory(filename))
                return ImportDirectory(filename);

            if (!IsMp3(filename)) 
                return Enumerable.Empty<Track>();

            Track track;
            if (TryGetTrack(filename, out track))
                return new[] { track };

            track = loader.Load(filename);
            storage.Add(track);
            messenger.Send(new TrackAddedToLibraryEvent(track));
            return new[] { track };
        }

        bool TryGetTrack(string filename, out Track track)
        {
            track = storage.Tracks.FirstOrDefault(t => t.Filename.Equals(filename, Comparison));

            return track != null;
        }

        static bool IsMp3(string filename)
        {
            var extension = Path.GetExtension(filename) ?? "";
            return extension.Equals(".mp3", Comparison);
        }

        public IEnumerable<Track> Import(IEnumerable<string> filenames)
        {
            if (filenames == null) throw new ArgumentNullException("filenames");

            return filenames.SelectMany(Import).ToList(); // force evaluation now
        }

        public IEnumerable<Track> ImportDirectory(string directoryName)
        {
            if (directoryName == null) throw new ArgumentNullException("directoryName");

            var filenames = Directory.GetFiles(directoryName, 
                "*.mp3", SearchOption.AllDirectories);

            return Import(filenames);
        }

        static bool IsDirectory(string filename)
        {
            var attributes = File.GetAttributes(filename);
            return (attributes & FileAttributes.Directory) == FileAttributes.Directory;
        }

        public void Remove(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            storage.Remove(track);
            messenger.Send(new TrackRemovedFromLibraryEvent(track));
        }
    }
}