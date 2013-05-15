using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MixPlanner.Events;
using MixPlanner.Loader;
using MixPlanner.Storage;

namespace MixPlanner.DomainModel
{
    public class TrackLibrary : ITrackLibrary
    {
        const StringComparison Comparison = StringComparison.CurrentCultureIgnoreCase;
        readonly ITrackLoader loader;
        readonly IDispatcherMessenger messenger;
        readonly ILibraryStorage storage;
        readonly IRecommendedTransitionDetector transitionDetector;
        readonly List<Track> tracks; 

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
            tracks = new List<Track>();
        }

        public IEnumerable<Tuple<Track, Transition>> GetRecommendations(IMixItem mixItem)
        {
            if (mixItem == null) throw new ArgumentNullException("mixItem");

            return tracks
                .Except(new[] {mixItem.Track}) // don't recommend itself!
                .Select(t => Tuple.Create(t, GetTransition(mixItem, t)))
                .Where(t => t.Item2 != null);
        }

        public async Task SaveAsync(Track track)
        {
            await storage.UpdateAsync(track);
            messenger.SendToUI(new TrackUpdatedEvent(track));
        }

        Transition GetTransition(IMixItem mixItem, Track track)
        {
            return transitionDetector.GetTransitionBetween(mixItem.PlaybackSpeed,
                                                           track.GetDefaultPlaybackSpeed());
        }

        bool TryGetTrack(string filename, out Track track)
        {
            track = tracks.FirstOrDefault(t => t.Filename.Equals(filename, Comparison));

            return track != null;
        }

        async Task<IEnumerable<Track>> ImportDirectory(string directoryName, 
            CancellationToken cancellationToken, IProgress<string> progress)
        {
            if (directoryName == null) throw new ArgumentNullException("directoryName");
            if (progress == null) throw new ArgumentNullException("progress");

            cancellationToken.ThrowIfCancellationRequested();

            messenger.SendToUI(new BeganScanningDirectoryEvent());

            progress.ReportFormat("Scanning {0}...", Path.GetFileName(directoryName));

            var filenames = Directory
                .GetFiles(directoryName, "*.*", SearchOption.AllDirectories)
                .Where(loader.IsSupportedFileFormat);

            return await ImportAsync(filenames, cancellationToken, progress)
                .ContinueWith<IEnumerable<Track>>(OnImportComplete);
        }

        IEnumerable<Track> OnImportComplete(Task<IEnumerable<Track>> task)
        {
            messenger.SendToUI(new FinishedLoadingTracksEvent());
            return task.Result;
        }

        public async Task<IEnumerable<Track>> ImportAsync(string filename,
            CancellationToken cancellationToken, IProgress<string> progress)
        {
            if (filename == null) throw new ArgumentNullException("filename");

            cancellationToken.ThrowIfCancellationRequested();

            if (IsDirectory(filename))
                return await ImportDirectory(filename, cancellationToken, progress);

            if (!loader.IsSupportedFileFormat(filename))
                return Enumerable.Empty<Track>();

            progress.ReportFormat("Loading {0}", Path.GetFileName(filename));

            Track track;
            if (TryGetTrack(filename, out track))
                return new[] { track };
            
            track = await loader.LoadAsync(filename);
            await storage.AddAsync(track);
            tracks.Add(track);
            messenger.SendToUI(new TrackAddedToLibraryEvent(track));
            return new[] { track };
        }

        public async Task<IEnumerable<Track>> ImportAsync(IEnumerable<string> filenames,
            CancellationToken cancellationToken, IProgress<string> progress)
        {
            var tracks = new List<Track>();
            foreach (var filename in filenames)
            {
                if (cancellationToken.IsCancellationRequested)
                    return tracks;

                tracks.AddRange(await ImportAsync(filename, cancellationToken, progress));
            }

            return tracks;
        }

        public async Task<IEnumerable<Track>> ImportDirectoryAsync(string directoryName,
            CancellationToken cancellationToken, IProgress<string> progress)
        {
            if (cancellationToken.IsCancellationRequested)
                return Enumerable.Empty<Track>();

            return await Task.Run(() => ImportDirectory(directoryName, cancellationToken, progress));
        }

        public async Task InitializeAsync()
        {
            tracks.AddRange(await storage.FetchAllAsync());
            messenger.SendToUI(new TrackLibraryLoadedEvent(tracks));
        }

        static bool IsDirectory(string filename)
        {
            var attributes = File.GetAttributes(filename);
            return (attributes & FileAttributes.Directory) == FileAttributes.Directory;
        }

        public async Task RemoveAsync(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            tracks.Remove(track);
            await storage.RemoveAsync(track);
            messenger.SendToUI(new TrackRemovedFromLibraryEvent(track));
        }

        public async Task RemoveRangeAsync(IEnumerable<Track> tracks)
        {
            if (tracks == null) throw new ArgumentNullException("tracks");
            foreach (var track in tracks)
                await RemoveAsync(track);
        }
    }
}