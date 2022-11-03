namespace Proof.Core.Images
{
    public record class CoreImageData(
        int Width,
        int Height,
        byte bitDepth,
        ColourType colourType,
        CompressionMethod compressionMethod,
        FilterMethod filterMethod,
        InterlaceMethod interlaceMethod);
}
