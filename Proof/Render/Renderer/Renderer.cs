using Proof.Core.Logging;
using Proof.Render.Buffers;
using Proof.Render.Textures;

namespace Proof.Render.Renderer
{
    public sealed class Renderer : IRenderer
    {
        private readonly ALogger _logger;

        private readonly VertexBuffer _vertexBuffer;
        private readonly IndexBuffer _indexBuffer;

        private readonly SortedDictionary<int, Layer> _submitted;

        public Renderer(ALogger logger, int vertexBufferCapacity, int indexBufferCapacity)
        {
            _logger = logger;

            _logger.LogInfo("Creating renderer...");

            _vertexBuffer = new VertexBuffer(logger, vertexBufferCapacity);
            _indexBuffer = new IndexBuffer(logger, indexBufferCapacity);
            _submitted = new SortedDictionary<int, Layer>();

            _logger.LogInfo("Renderer created.");
        }

        public void Submit(float[] vertices, int[] indices, int layerIndex, ITexture texture)
        {
            var renderData = new RenderData
            {
                Vertices = vertices,
                Indices = indices,
            };

            if (_submitted.TryGetValue(layerIndex, out Layer? layer))
            {
                layer.Add(renderData, texture);
            }
            else
            {
                var newLayer = new Layer();
                newLayer.Add(renderData, texture);
                _submitted.Add(layerIndex, newLayer);
            }
        }

        public void Flush(VertexLayout layout)
        {
            foreach(Layer layer in _submitted.Values)
            {
                layer.Render(layout, _vertexBuffer, _indexBuffer);
            }

            _vertexBuffer.Flush(layout);
            _indexBuffer.Flush();
            _submitted.Clear();
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
