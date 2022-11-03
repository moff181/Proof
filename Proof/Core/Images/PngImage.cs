namespace Proof.Core.Images
{
    public class PngImage
    {
        public PngImage(CoreImageData coreImageData, byte[] data)
        {
            CoreImageData = coreImageData;
            Data = data;
        }

        public CoreImageData CoreImageData { get; }
        
        public byte[] Data { get; }
    }
}
