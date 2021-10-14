using System;
using CodeBase.DataStructures.ObjectPool;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Towers.Projectiles
{
    [SelectionBase]
    public class Projectile : MonoBehaviour, IObjectPoolItem<Projectile>
    {
        [Header("Refs")]
        [SerializeField] private TriggerObserver _trigger;
        [SerializeField] private ProjectileFly _fly;
        [Space]
        [SerializeField] private float _damage;

        private int _unitMask;

        private GenericObjectPool<Projectile> _nativePool => (this as IObjectPoolItem<Projectile>).NativePool;

        GenericObjectPool<Projectile> IObjectPoolItem<Projectile>.NativePool { get; set; }

        private void Start()
        {
            _unitMask = 1 << LayerMask.NameToLayer("RedUnit");
        }

        public void Launch(Transform target, TrajectoryData trajectoryData)
        {
            _fly.FlyTo(target, trajectoryData);
        }

        private void OnEnable() => _trigger.Entered += Damage;

        private void OnDisable() => _trigger.Entered -= Damage;


        private void Damage(Collider obj)
        {
            if (obj.transform.parent.TryGetComponent(out IHealth health))
            {
                health.TakeDamage(_damage);
                _nativePool.ReturnToPool(this);   
            }
        }
    }
}