using System;
using System.Collections.Generic;
using System.IO;
using GalaSoft.MvvmLight.Messaging;
using MixPlanner.DomainModel;
using MixPlanner.Events;
using MixPlanner.Mp3;
using MoreLinq;

namespace MixPlanner.Storage
{
    public class InMemoryTrackLibrary : ITrackLibrary
    {
        readonly ITrackLoader loader;
        readonly IMessenger messenger;

        readonly IList<Track> tracks;
        public IEnumerable<Track> Tracks { get { return tracks; } }

        public InMemoryTrackLibrary(ITrackLoader loader, IMessenger messenger)
        {
            if (loader == null) throw new ArgumentNullException("loader");
            if (messenger == null) throw new ArgumentNullException("messenger");
            this.loader = loader;
            this.messenger = messenger;
            tracks = new List<Track>();
        }

        public void Import(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");
            var track = loader.Load(filename);
            tracks.Add(track);
            messenger.Send(new TrackAddedToLibraryEvent(track));
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

        public void Remove(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            tracks.Remove(track);
            messenger.Send(new TrackRemovedFromLibraryEvent(track));
        }
    }
}