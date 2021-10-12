using System;
using CodeBase.DataStructures.ObjectPool;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Towers
{
    [SelectionBase]
    public class Projectile : MonoBehaviour, IObjectPoolItem<Projectile>
    {
        [Header("Refs")] [SerializeField] private TriggerObserver _trigger;
        [Space]
        [SerializeField] private float _speed;
        [SerializeField] private float _damage;
        private Transform _target;

        private GenericObjectPool<Projectile> _nativePool => (this as IObjectPoolItem<Projectile>).NativePool;

        GenericObjectPool<Projectile> IObjectPoolItem<Projectile>.NativePool { get; set; }

        public void Launch(Transform target) => _target = target;

        private void OnEnable() => _trigger.Entered += Damage;

        private void OnDisable() => _trigger.Entered -= Damage;


        private void Update() 
            => transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * _speed);

        private void Damage(Collider obj)
        {
            Debug.Log($"Damage = {obj.name}");
            _nativePool.ReturnToPool(this);
        }
    }
}