using Proof.Core.Logging;
using Proof.OpenGL;
using Proof.Render.Buffers;
using Proof.Render.Shaders;
using Proof.Render.Textures;

namespace Proof.Render.Renderer
{
    public sealed class Renderer : IRenderer
    {
        private readonly ALogger _logger;

        private readonly VertexBuffer _vertexBuffer;
        private readonly IndexBuffer _indexBuffer;

        private readonly SortedDictionary<int, Layer> _submitted;

        public Renderer(IOpenGLApi gl, ALogger logger, int vertexBufferCapacity, int indexBufferCapacity)
        {
            _logger = logger;

            _logger.LogInfo("Creating renderer...");

            _vertexBuffer = new VertexBuffer(gl, logger, vertexBufferCapacity);
            _indexBuffer = new IndexBuffer(gl, logger, indexBufferCapacity);
            _submitted = new SortedDictionary<int, Layer>();
            gl.Enable(GLConstants.GL_BLEND);
            gl.BlendFunc(GLConstants.GL_SRC_ALPHA, GLConstants.GL_ONE_MINUS_SRC_ALPHA);

            _logger.LogInfo("Renderer created.");
        }

        public void Submit(float[] vertices, int[] indices, int layerIndex, ITexture texture, IShader shader)
        {
            var renderData = new RenderData
            {
                Vertices = vertices,
                Indices = indices,
            };

            if (_submitted.TryGetValue(layerIndex, out Layer? layer))
            {
                layer.Add(renderData, texture, shader);
            }
            else
            {
                var newLayer = new Layer();
                newLayer.Add(renderData, texture, shader);
                _submitted.Add(layerIndex, newLayer);
            }
        }

        public void Flush(VertexLayout layout)
        {
            int textureSlot = 0;
            foreach(Layer layer in _submitted.Values)
            {
                layer.Render(_vertexBuffer, _indexBuffer, ref textureSlot);
            }

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
