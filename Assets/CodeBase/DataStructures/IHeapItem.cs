using System;

namespace CodeBase.DataStructures
{
    public interface IHeapItem<in T> : IComparable<T>
    {
        public int HeapIndex { get; set; }
    }
}