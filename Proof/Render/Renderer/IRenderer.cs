using Proof.Render.Buffers;
using Proof.Render.Shaders;
using Proof.Render.Textures;

namespace Proof.Render.Renderer
{
    public interface IRenderer : IDisposable
    {
        void Flush(VertexLayout layout);
        void Submit(float[] vertices, int[] indices, int layerIndex, ITexture texture, IShader shader);
    }
}