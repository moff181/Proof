using Proof.Core.DataStructures;
using Proof.Core.Logging;
using Proof.OpenGL;

namespace Proof.Render.Buffers
{
    public sealed class VertexBuffer : IDisposable
    {
        private readonly IOpenGLApi _gl;
        private readonly ALogger _logger;

        private readonly uint _vaoId;
        private readonly uint _vboId;

        private readonly FixedList<float> _vertices;

        public VertexBuffer(IOpenGLApi gl, ALogger logger, int capacity)
        {
            _gl = gl;
            _logger = logger;

            _logger.LogInfo($"Creating vertex buffer with capacity of {capacity}");

            _vertices = new FixedList<float>(capacity);
            _vaoId = _gl.GenVertexArray();
            _vboId = _gl.GenBuffer();

            _logger.LogInfo($"Vertex buffer generated successfully (VAO ID: {_vaoId}; VBO ID: {_vboId})");
        }

        public void Dispose()
        {
            _logger.LogInfo($"Disposing of vertex buffer (VAO ID: {_vaoId}; VBO ID: {_vboId})");

            _gl.DeleteBuffer(_vboId);
            _gl.DeleteVertexArray(_vaoId);

            _logger.LogInfo("Vertex buffer disposed of.");
        }

        public void Submit(float[] vertices)
        {
            _vertices.Add(vertices);
        }

        public unsafe void Flush(VertexLayout layout)
        {
            Bind(layout);

            fixed (void* ptr = &_vertices.First())
            {
                _gl.BufferData(GLConstants.GL_ARRAY_BUFFER, sizeof(float) * _vertices.Index, ptr, GLConstants.GL_STATIC_DRAW);
            }

            _vertices.Clear();
        }

        private unsafe void Bind(VertexLayout layout)
        {
            _gl.BindVertexArray(_vaoId);
            _gl.BindBuffer(GLConstants.GL_ARRAY_BUFFER, _vboId);

            uint counter = 0;
            int total = 0;
            foreach (int arraySize in layout)
            {
                _gl.VertexAttribPointer(counter, arraySize, GLConstants.GL_FLOAT, false, layout.Stride(), (void*)(total * sizeof(float)));
                _gl.EnableVertexAttribArray(counter);

                total += arraySize;
                counter++;
            }
        }
    }
}
