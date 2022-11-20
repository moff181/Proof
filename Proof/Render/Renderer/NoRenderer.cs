using Proof.Render.Shaders;
using Proof.Render.Textures;

namespace Proof.Render.Renderer
{
    public sealed class NoRenderer : IRenderer
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Flush()
        {
            throw new NotImplementedException();
        }

        public void Submit(float[] vertices, int[] indices, int layerIndex, ITexture texture, IShader shader)
        {
            throw new NotImplementedException();
        }
    }
}
