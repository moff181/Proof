using Proof.Core.DataStructures;
using Proof.Core.Logging;
using Proof.OpenGL;

namespace Proof.Render.Buffers
{
    public sealed class IndexBuffer : IDisposable
    {
        private readonly IOpenGLApi _gl;
        private readonly ALogger _logger;

        private readonly FixedList<int> _indices;

        private int _largestIndex;

        public IndexBuffer(IOpenGLApi gl, ALogger logger, int capacity)
        {
            _gl = gl;
            _logger = logger;
            _indices = new FixedList<int>(capacity);
            _largestIndex = 0;

            _logger.LogInfo($"Generated index buffer with capacity of {capacity}");
        }

        public void Dispose()
        {
            _logger.LogInfo("Index buffer disposed of.");
        }

        public void Submit(int[] indices)
        {
            int largestIndex = 0;

            foreach (int i in indices)
            {
                int index = i + _largestIndex;
                if (index > largestIndex)
                {
                    largestIndex = index;
                }

                _indices.Add(index);
            }

            _largestIndex = largestIndex + 1;
        }

        public unsafe void Flush()
        {
            fixed (void* ptr = &_indices.First())
            {
                _gl.DrawElements(GLConstants.GL_TRIANGLES, _indices.Index, GLConstants.GL_UNSIGNED_INT, ptr);
            }

            _indices.Clear();
            _largestIndex = 0;
        }
    }
}
