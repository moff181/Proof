namespace Proof.Render.Textures
{
    public class NoTexture : ITexture
    {
        public static NoTexture Instance { get; } = new NoTexture();

        private NoTexture()
        { }

        public string FilePath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Bind(int textureSlot)
        {
            // Do nothing
        }
    }
}
