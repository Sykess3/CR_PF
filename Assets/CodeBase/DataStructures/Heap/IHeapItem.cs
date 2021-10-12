using System;

namespace CodeBase.DataStructures.Heap
{
    public interface IHeapItem<in T> : IComparable<T>
    {
        public int HeapIndex { get; set; }
    }
}