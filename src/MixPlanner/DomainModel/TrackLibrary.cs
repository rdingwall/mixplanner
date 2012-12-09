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

        public void Import(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");

            if (IsDirectory(filename))
            {
                ImportDirectory(filename);
                return;
            }

            if (!IsMp3(filename)) return;
            if (AlreadyContains(filename)) return;

            var track = loader.Load(filename);
            storage.Add(track);
            messenger.Send(new TrackAddedToLibraryEvent(track));
        }

        bool AlreadyContains(string filename)
        {
            return storage.Tracks.Any(t => t.Filename.Equals(filename, Comparison));
        }

        static bool IsMp3(string filename)
        {
            var extension = Path.GetExtension(filename) ?? "";
            return extension.Equals(".mp3", Comparison);
        }

        public void Import(IEnumerable<string> filenames)
        {
            if (filenames == null) throw new ArgumentNullException("filenames");

            filenames.ForEach(Import);
        }

        public void ImportDirectory(string directoryName)
        {
            if (directoryName == null) throw new ArgumentNullException("directoryName");

            var filenames = Directory.GetFiles(directoryName, 
                "*.mp3", SearchOption.AllDirectories);

            Import(filenames);
        }

        bool IsDirectory(string filename)
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