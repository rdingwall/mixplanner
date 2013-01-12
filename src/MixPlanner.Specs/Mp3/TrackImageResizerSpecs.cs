using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
using Machine.Specifications;
using MixPlanner.Mp3;

namespace MixPlanner.Specs.Mp3
{
    [Subject(typeof(TrackImageResizer))]
    public class TrackImageResizerSpecs
    {
        Establish context =
            () =>
                {
                    using (var stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStreamWithNameEnding("resizeTest.jpg"))
                    using (var memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        originalData = memoryStream.GetBuffer();
                    }

                };

        Because of = () => imageSet = new TrackImageResizer().Process(originalData);

        It should_leave_the_original = 
            () => imageSet.FullSize.Data.ShouldBeTheSameAs(originalData);

        It should_resize_to_24_x_24 = () => ShouldBeImageOf(imageSet.Resized24X24, 24, 24);

        It should_resize_to_64_x_64 = () => ShouldBeImageOf(imageSet.Resized64X64, 64, 64);

        static byte[] originalData;
        static TrackImageData imageSet;

        static void ShouldBeImageOf(ImageData buffer, int maxWidth, int maxHeight)
        {
            buffer.ShouldNotBeNull();
            buffer.Data.ShouldNotBeEmpty();
            using (var stream = new MemoryStream(buffer.Data))
            using (var image = Image.FromStream(stream))
            {
                image.Width.ShouldBeLessThanOrEqualTo(maxWidth);
                image.Height.ShouldBeLessThanOrEqualTo(maxHeight);
            }
        }
    }
}