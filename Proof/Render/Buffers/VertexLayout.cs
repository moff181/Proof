namespace Proof.Render.Buffers
{
    internal class VertexLayout
    {
        private readonly IList<int> _arraySizes;

        public VertexLayout()
        {
            _arraySizes = new List<int>();
        }

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
    }
}
