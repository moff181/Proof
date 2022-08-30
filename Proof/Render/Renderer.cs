using Proof.Core.Logging;
using Proof.Render.Buffers;

namespace Proof.Render
{
    public class Renderer : IDisposable
    {
        private readonly ALogger _logger;

        private readonly VertexBuffer _vertexBuffer;
        private readonly IndexBuffer _indexBuffer;

        public Renderer(ALogger logger, int vertexBufferCapacity, int indexBufferCapacity)
        {
            _logger = logger;

            _logger.LogInfo("Creating renderer...");

            _vertexBuffer = new VertexBuffer(logger, vertexBufferCapacity);
            _indexBuffer = new IndexBuffer(logger, indexBufferCapacity);

            _logger.LogInfo("Renderer created.");
        }

        public void Submit(float[] vertices, int[] indices)
        {
            _vertexBuffer.Submit(vertices);
            _indexBuffer.Submit(indices);
        }

        public void Flush(VertexLayout layout)
        {
            _vertexBuffer.Flush(layout);
            _indexBuffer.Flush();
        }

        public void Dispose()
        {
            _logger.LogInfo("Disposing of renderer...");

            _vertexBuffer.Dispose();
            _indexBuffer.Dispose();

            _logger.LogInfo("Renderer disposed of.");
        }
    }
}
