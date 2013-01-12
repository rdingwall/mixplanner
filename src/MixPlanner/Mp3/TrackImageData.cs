using System;

namespace MixPlanner.Mp3
{
    public class TrackImageData
    {
        public TrackImageData(byte[] fullSizeData, byte[] resized64X64Data, byte[] resized24X24Data)
        {
            if (fullSizeData == null) throw new ArgumentNullException("fullSizeData");
            if (resized64X64Data == null) throw new ArgumentNullException("resized64X64Data");
            if (resized24X24Data == null) throw new ArgumentNullException("resized24X24Data");
            FullSize = new ImageData(fullSizeData);
            Resized64X64 = new ImageData(resized64X64Data);
            Resized24X24 = new ImageData(resized24X24Data);
        }

        // Hopefully 300x300 or 500x500
        public ImageData FullSize { get; private set; }
        public ImageData Resized64X64 { get; private set; }
        public ImageData Resized24X24 { get; private set; }
    }
}