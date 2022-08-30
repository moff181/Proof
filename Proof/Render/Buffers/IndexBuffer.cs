using Proof.Core.DataStructures;
using Proof.Core.Logging;
using Proof.OpenGL;
using System;

namespace Proof.Render.Buffers
{
    public class IndexBuffer : IDisposable
    {
        private readonly ALogger _logger;

        private readonly FixedList<int> _indices;

        private int _largestIndex;

        public IndexBuffer(ALogger logger, int capacity)
        {
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
                GL.glDrawElements(GL.GL_TRIANGLES, _indices.Index, GL.GL_UNSIGNED_INT, ptr);
            }

            _indices.Clear();
            _largestIndex = 0;
        }
    }
}
