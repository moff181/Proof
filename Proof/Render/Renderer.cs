using Proof.Core.Logging;
using Proof.Render.Buffers;

namespace Proof.Render
{
    public class Renderer : IDisposable
    {
        private readonly ALogger _logger;

        private readonly VertexBuffer _vertexBuffer;
        private readonly IndexBuffer _indexBuffer;

        private readonly SortedDictionary<int, List<RenderData>> _submitted;

        public Renderer(ALogger logger, int vertexBufferCapacity, int indexBufferCapacity)
        {
            _logger = logger;

            _logger.LogInfo("Creating renderer...");

            _vertexBuffer = new VertexBuffer(logger, vertexBufferCapacity);
            _indexBuffer = new IndexBuffer(logger, indexBufferCapacity);
            _submitted = new SortedDictionary<int, List<RenderData>>();

            _logger.LogInfo("Renderer created.");
        }

        public void Submit(float[] vertices, int[] indices, int layer)
        {
            var renderData = new RenderData
            {
                Vertices = vertices,
                Indices = indices,
            };

            if (_submitted.TryGetValue(layer, out List<RenderData>? list))
            {
                list.Add(renderData);
            }
            else
            {
                var newList = new List<RenderData>();
                newList.Add(renderData);
                _submitted.Add(layer, newList);
            }
        }

        public void Flush(VertexLayout layout)
        {
            foreach (KeyValuePair<int, List<RenderData>> layer in _submitted)
            {
                foreach (var renderData in layer.Value)
                {
                    _vertexBuffer.Submit(renderData.Vertices);
                    _indexBuffer.Submit(renderData.Indices);
                }
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
