using Proof.Render.Buffers;
using Proof.Render.Textures;

namespace Proof.Render.Renderer
{
    public sealed class NoRenderer : IRenderer
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Flush(VertexLayout layout)
        {
            throw new NotImplementedException();
        }

        public void Submit(float[] vertices, int[] indices, int layer, Texture texture)
        {
            throw new NotImplementedException();
        }
    }
}
