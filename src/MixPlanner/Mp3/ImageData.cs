using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MixPlanner.Mp3
{
    public class ImageData
    {
        readonly Lazy<ImageSource> imageSource; 

        public byte[] Data { get; private set; }
        public ImageSource ImageSource { get { return imageSource.Value; } }

        public ImageData(byte[] imageData)
        {
            Data = imageData;
            imageSource = new Lazy<ImageSource>(CreateImageSource);
        }

        ImageSource CreateImageSource()
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            using (var stream = new MemoryStream(Data))
            {
                bitmap.StreamSource = stream;
                bitmap.EndInit();
            }
            return bitmap;
        }
    }
}