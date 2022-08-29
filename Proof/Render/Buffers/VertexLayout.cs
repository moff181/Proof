using System.Collections;

namespace Proof.Render.Buffers
{
    internal class VertexLayout : IEnumerable<int>
    {
        private readonly IList<int> _arraySizes;

        public VertexLayout(int positionIndex)
        {
            _arraySizes = new List<int>();
            PositionIndex = positionIndex;
        }

        public int PositionIndex { get; }

        public void AddArray(int arraySize)
        {
            _arraySizes.Add(arraySize);
        }

        public int Count()
        {
            return _arraySizes.Count;
        }

        public int SumOfElements()
        {
            return _arraySizes.Sum();
        }

        public int Stride()
        {
            return SumOfElements() * sizeof(float);
        }

        public IEnumerator<int> GetEnumerator()
        {
            return _arraySizes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
