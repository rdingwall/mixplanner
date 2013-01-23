using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MixPlanner.Loader
{
    public interface ITrackImageResizer
    {
        TrackImageData Process(byte[] fullSizeImageData);
    }

    public class TrackImageResizer : ITrackImageResizer
    {
        public TrackImageData Process(byte[] fullSizeImageData)
        {
            if (fullSizeImageData == null) throw new ArgumentNullException("fullSizeImageData");

            using (var stream = new MemoryStream(fullSizeImageData))
            using (var original = Image.FromStream(stream))
            {
                var resized64x64 = GetResizedData(original, 64, 64);
                var resized24x24 = GetResizedData(original, 24, 24);

                return new TrackImageData(fullSizeImageData, resized64x64, resized24x24);
            }
        }

        static byte[] GetResizedData(Image original, int maxWidth, int maxHeight)
        {
            using (var resizedImage = ScaleImage(original, maxWidth, maxHeight))
            using (var stream = new MemoryStream())
            {
                resizedImage.Save(stream, ImageFormat.Png);
                return stream.GetBuffer();
            }
        }

        // From http://stackoverflow.com/a/6501997/91551
        static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }
    }
}