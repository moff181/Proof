using System.Drawing;
using System.Runtime.Versioning;

namespace Proof.Core.Extensions
{
    public static class ImageExtensions
    {
        [SupportedOSPlatform("windows")]
        public static byte[] ToByteArray(this Image image)
        {
            using var memoryStream = new MemoryStream();
            image.Save(memoryStream, image.RawFormat);
            return memoryStream.ToArray();
        }
    }
}
