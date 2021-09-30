using System;

namespace CodeBase.DataStructures
{
    public class Heap<T> where T : IHeapItem<T>
    {
        private T[] _items;

        public int Count { get; private set; }

        public Heap(int maxSize)
        {
            _items = new T[maxSize];
        }

        public void Add(T item)
        {
            _items[Count] = item;
            item.HeapIndex = Count;
            SortUp(item);
            Count++;
        }

        public T RemoveFirst()
        {
            T firstItem = _items[0];
            Count--;
            _items[0] = _items[Count];
            _items[0].HeapIndex = 0;
            SortDown(_items[0]);

            return firstItem;
        }

        public void UpdateItem(T item) =>
            SortUp(item);

        public bool Contains(T item) =>
            Equals(item, _items[item.HeapIndex]);

        public void Clear()
        {
            Array.Clear(_items, 0, Count);
            Count = 0;
        }
        private void SortDown(T item)
        {
            while (true)
            {
                int leftChildIndex = (item.HeapIndex * 2) + 1;
                int rightChildIndex = (item.HeapIndex * 2) + 2;

                if (leftChildIndex < Count)
                {
                    int spawnIndex = leftChildIndex;
                    if (_items[leftChildIndex].CompareTo(_items[rightChildIndex]) < 0)
                        spawnIndex = rightChildIndex;

                    if (item.CompareTo(_items[spawnIndex]) < 0)
                        Swap(_items[spawnIndex], item);
                    else
                        return;
                }
                else
                {
                    return;
                }
            }
        }

        private void SortUp(T item)
        {
            while (true)
            {
                int parentIndex = (Count - 1) / 2;

                if (item.CompareTo(_items[parentIndex]) > 0)
                    Swap(item, _items[parentIndex]);
                else
                    return;
            }
        }

        private void Swap(T item1, T item2)
        {
            _items[item1.HeapIndex] = item2;
            _items[item2.HeapIndex] = item1;

            int item1Index = item1.HeapIndex;
            item1.HeapIndex = item2.HeapIndex;
            item2.HeapIndex = item1Index;
        }
    }
}