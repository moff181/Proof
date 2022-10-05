using Proof.Render.Buffers;

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

        public void Submit(float[] vertices, int[] indices, int layer)
        {
            throw new NotImplementedException();
        }
    }
}
