using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        readonly IDispatcherMessenger messenger;
        readonly ILibraryStorage storage;
        readonly IRecommendedTransitionDetector transitionDetector;

        public TrackLibrary(
            ITrackLoader loader, 
            IDispatcherMessenger messenger, 
            ILibraryStorage storage,
            IRecommendedTransitionDetector transitionDetector)
        {
            if (loader == null) throw new ArgumentNullException("loader");
            if (messenger == null) throw new ArgumentNullException("messenger");
            if (storage == null) throw new ArgumentNullException("storage");
            if (transitionDetector == null) throw new ArgumentNullException("transitionDetector");
            this.loader = loader;
            this.messenger = messenger;
            this.storage = storage;
            this.transitionDetector = transitionDetector;
        }

        public IEnumerable<Tuple<Track, Transition>> GetNextTracks(MixItem mixItem)
        {
            if (mixItem == null) throw new ArgumentNullException("mixItem");

            return storage.Tracks
                          .Except(new[] {mixItem.Track}) // don't recommend itself!
                          .Select(t => Tuple.Create(t, GetTransition(mixItem, t)))
                          .Where(t => t.Item2 != null);
        }

        Transition GetTransition(MixItem mixItem, Track track)
        {
            return transitionDetector.GetTransitionBetween(mixItem.PlaybackSpeed,
                                                           track.GetDefaultPlaybackSpeed());
        }

        IEnumerable<Track> Import(string filename)
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
            messenger.SendToUI(new TrackAddedToLibraryEvent(track));
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

        IEnumerable<Track> Import(IEnumerable<string> filenames)
        {
            if (filenames == null) throw new ArgumentNullException("filenames");

            messenger.SendToUI(new BeganLoadingTracksEvent());

            var tracks = filenames.SelectMany(Import).ToList(); // force evaluation now

            messenger.SendToUI(new FinishedLoadingTracksEvent());

            return tracks;
        }

        IEnumerable<Track> ImportDirectory(string directoryName)
        {
            if (directoryName == null) throw new ArgumentNullException("directoryName");

            messenger.SendToUI(new BeganScanningDirectoryEvent());

            var filenames = Directory.GetFiles(directoryName, 
                "*.mp3", SearchOption.AllDirectories);

            return Import(filenames);
        }

        public async Task<IEnumerable<Track>> ImportAsync(string filename)
        {
            return await Task.Run(() => Import(filename));
        }

        public async Task<IEnumerable<Track>> ImportAsync(IEnumerable<string> filenames)
        {
            return await Task.Run(() => Import(filenames));
        }

        public async Task<IEnumerable<Track>> ImportDirectoryAsync(string directoryName)
        {
            return await Task.Run(() => ImportDirectory(directoryName));
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
            messenger.SendToUI(new TrackRemovedFromLibraryEvent(track));
        }

        public void RemoveRange(IEnumerable<Track> tracks)
        {
            if (tracks == null) throw new ArgumentNullException("tracks");
            tracks.ForEach(Remove);
        }
    }
}