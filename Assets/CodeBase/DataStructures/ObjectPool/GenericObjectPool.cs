using System.Collections.Generic;
using CodeBase.Logic;
using JetBrains.Annotations;
using UnityEngine;

namespace CodeBase.DataStructures.ObjectPool
{
    public abstract class GenericObjectPool<T> where T : MonoBehaviour, IObjectPoolItem<T>
    {
        private Queue<T> _objects;
        private T _prefab;
        private CarrierToAdditiveScene _carrierToAdditiveScene;

        protected GenericObjectPool(T prefab, [NotNull] string additiveSceneName, int count = 0)
        {
            _objects = new Queue<T>();
            _carrierToAdditiveScene = new CarrierToAdditiveScene(additiveSceneName);
            _prefab = prefab;
            AddObjects(count);
        }

        public T Get()
        {
            if (_objects.Count == 0) 
                AddObjects(1);

            var objectPoolItem = _objects.Dequeue();
            objectPoolItem.gameObject.SetActive(true);
            return objectPoolItem;
        }

        public void ReturnToPool(T objectToReturn)
        {
            objectToReturn.gameObject.SetActive(false);
            _objects.Enqueue(objectToReturn);
        }

        private void AddObjects(int count)
        {
            for (int i = 0; i < count; i++)
            {
                T objectPoolItem = Object.Instantiate(_prefab);
                _carrierToAdditiveScene.MoveInstanceToScene(objectPoolItem);

                objectPoolItem.NativePool = this;
                objectPoolItem.gameObject.SetActive(false);
                _objects.Enqueue(objectPoolItem);
            }
        }
    }
}