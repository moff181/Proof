using System.Drawing;
using System.Runtime.Versioning;

namespace Proof.Core.Extensions
{
    public static class BitmapExtensions
    {
        [SupportedOSPlatform("windows")]
        public static byte[] ToByteArray(this Bitmap image)
        {
            int pixels = image.Width * image.Height;
            var bytes = new byte[pixels * 4];

            for(int i = 0; i < pixels; i++)
            {
                var pixel = image.GetPixel(i % image.Width, i / image.Height);
                bytes[i * 4 + 0] = pixel.R;
                bytes[i * 4 + 1] = pixel.G;
                bytes[i * 4 + 2] = pixel.B;
                bytes[i * 4 + 3] = pixel.A;
            }

            return bytes;
        }
    }
}
