namespace Proof.Core.DataStructures
{
    public class FixedList<T> where T : unmanaged
    {
        private readonly T[] _items;

        public FixedList(int size)
        {
            _items = new T[size];
            Index = 0;
        }

        public int Index { get; private set; }

        public void Add(T[] toAdd)
        {
            if (toAdd.Length > 100)
            {
                Array.Copy(toAdd, 0, _items, Index, toAdd.Count());
            }
            else
            {
                for (int i = 0; i < toAdd.Length; i++)
                {
                    _items[i + Index] = toAdd[i];
                }
            }
            Index += toAdd.Length;
        }

        public void Add(T toAdd)
        {
            _items[Index++] = toAdd;
        }

        public int Capacity()
        {
            return _items.Length;
        }

        public void Clear()
        {
            Index = 0;
        }

        public ref readonly T First()
        {
            return ref _items[0];
        }

        public T Get(int index)
        {
            if(index >= Index)
            {
                throw new ArgumentOutOfRangeException($"Provided index ({index}) must be less than Index.");
            }

            return _items[index];
        }
    }
}
