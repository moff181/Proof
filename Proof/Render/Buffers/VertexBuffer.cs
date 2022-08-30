using Proof.Core.DataStructures;
using Proof.Core.Logging;
using Proof.OpenGL;

namespace Proof.Render.Buffers
{
    public class VertexBuffer : IDisposable
    {
        private readonly ALogger _logger;

        private readonly uint _vaoId;
        private readonly uint _vboId;

        private readonly FixedList<float> _vertices;

        public VertexBuffer(ALogger logger, int capacity)
        {
            _logger = logger;

            _logger.LogInfo($"Creating vertex buffer with capacity of {capacity}");

            _vertices = new FixedList<float>(capacity);
            _vaoId = GL.glGenVertexArray();
            _vboId = GL.glGenBuffer();

            _logger.LogInfo($"Vertex buffer generated successfully (VAO ID: {_vaoId}; VBO ID: {_vboId})");
        }

        public void Dispose()
        {
            _logger.LogInfo($"Disposing of vertex buffer (VAO ID: {_vaoId}; VBO ID: {_vboId})");

            GL.glDeleteBuffer(_vboId);
            GL.glDeleteVertexArray(_vaoId);

            _logger.LogInfo("Vertex buffer disposed of.");
        }

        public void Submit(float[] vertices)
        {
            _vertices.Add(vertices);
        }

        public unsafe void Flush(VertexLayout layout)
        {
            Bind(layout);

            fixed (void* ptr = _vertices.Items)
            {
                GL.glBufferData(GL.GL_ARRAY_BUFFER, sizeof(float) * _vertices.Index, ptr, GL.GL_STATIC_DRAW);
            }

            _vertices.Clear();
        }

        private unsafe void Bind(VertexLayout layout)
        {
            GL.glBindVertexArray(_vaoId);
            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, _vboId);

            uint counter = 0;
            int total = 0;
            foreach (int arraySize in layout)
            {
                GL.glVertexAttribPointer(counter, arraySize, GL.GL_FLOAT, false, layout.Stride(), (void*)(total * sizeof(float)));
                GL.glEnableVertexAttribArray(counter);

                total += arraySize;
                counter++;
            }
        }
    }
}
