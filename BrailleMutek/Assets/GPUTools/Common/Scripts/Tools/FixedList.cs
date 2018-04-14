namespace GPUTools.Common.Scripts.Tools
{
    public class FixedList<T>
    {
        public int Size { private set; get; }
        public int Count { private set; get; }

        public T[] Data { private set; get; }

        public FixedList(int size)
        {
            Data = new T[size];
            Size = size;
            Count = 0;
        }

        public void Add(T item)
        {
            Data[Count] = item;
            Count++;
        }

        public T this[int i]
        {
            set { Data[i] = value; }
            get { return Data[i]; }
        }

        public void Reset()
        {
            Count = 0;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (Data[i].Equals(item))
                    return true;
            }

            return false;
        }
    }
}
