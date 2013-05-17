using System;
using System.IO;
using MixPlanner.DomainModel;

namespace MixPlanner.Storage
{
    public interface IStorageFilenameFormatter
    {
        string FormatCoverArtFilename(Track track);
        string FormatTrackFilename(Track track);
        string GetCorrespondingImageFilename(string trackFilename);
    }

    public class StorageFilenameFormatter : IStorageFilenameFormatter
    {
        readonly string directory;
        readonly string imageExtension;

        public StorageFilenameFormatter(string directory, string imageExtension)
        {
            if (directory == null) throw new ArgumentNullException("directory");
            if (imageExtension == null) throw new ArgumentNullException("imageExtension");
            this.directory = directory;
            this.imageExtension = imageExtension;
        }

        public string FormatCoverArtFilename(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            return Path.Combine(directory, string.Format("{0}.{1}", track.Id, imageExtension));
        }

        public string FormatTrackFilename(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");
            return Path.Combine(directory, string.Format("{0}.track", track.Id));
        }

        public string GetCorrespondingImageFilename(string trackFilename)
        {
            if (trackFilename == null) throw new ArgumentNullException("trackFilename");
            string baseFilename = Path.GetFileNameWithoutExtension(trackFilename);
            return Path.Combine(directory, String.Format("{0}.{1}", baseFilename, imageExtension));
        }
    }
}