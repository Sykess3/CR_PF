using UnityEngine;

namespace CodeBase.DataStructures.ObjectPool
{
    public interface IObjectPoolItem<T> where T : MonoBehaviour, IObjectPoolItem<T>
    {
        GenericObjectPool<T> NativePool { get; set; }
    }
}