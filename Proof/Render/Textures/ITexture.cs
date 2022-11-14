namespace Proof.Render.Textures
{
    public interface ITexture
    {
        string FilePath { get; set; }

        void Bind(int textureSlot);
    }
}