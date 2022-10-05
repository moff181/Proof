using Proof.Render.Buffers;

namespace Proof.Render.Renderer
{
    public interface IRenderer : IDisposable
    {
        void Flush(VertexLayout layout);
        void Submit(float[] vertices, int[] indices, int layer);
    }
}