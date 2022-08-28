namespace Proof.Core.DataStructures
{
    internal class FixedList<T> where T : unmanaged
    {
        public FixedList(int size)
        {
            Items = new T[size];
            Index = 0;
        }

        public T[] Items { get; }

        public int Index { get; private set; }

        public void Add(T[] toAdd)
        {
            Array.Copy(toAdd, 0, Items, Index, toAdd.Count());
        }

        public void Add(T toAdd)
        {
            Items[Index++] = toAdd;
        }

        public int Capacity()
        {
            return Items.Length;
        }

        public void Clear()
        {
            Index = 0;
        }
    }
}
