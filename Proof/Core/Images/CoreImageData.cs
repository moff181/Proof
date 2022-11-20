namespace Proof.Core.Images
{
    public record class CoreImageData(
        int Width,
        int Height,
        byte BitDepth,
        ColourType ColourType,
        CompressionMethod CompressionMethod,
        FilterMethod FilterMethod,
        InterlaceMethod InterlaceMethod);
}
