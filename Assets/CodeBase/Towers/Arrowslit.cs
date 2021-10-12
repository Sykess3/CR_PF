using System;
using UnityEngine;

namespace CodeBase.Towers
{
    [SelectionBase]
    public class Arrowslit : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Transform _shootPoint;
        [Space] 
        
        [SerializeField] private string _projectilesSceneName;
        [SerializeField] private float _range;
        [SerializeField] private float _delay;
        [SerializeField] private Projectile _projectilePrefab;
        private ProjectilePool _projectilePool;

        private float _timeAfterShoot;
        
        public Transform Target { get; set; }

        private void Start()
        {
            _projectilePool = new ProjectilePool(_projectilePrefab, _projectilesSceneName);
        }

        private void Update()
        {
            _timeAfterShoot += Time.deltaTime;
            if (_timeAfterShoot >= _delay)
            {
                Shoot(Target);
                _timeAfterShoot -= _delay;
            }
        }

        private void Shoot(Transform target)
        {
            Projectile projectile = _projectilePool.Get();
            projectile.transform.SetPositionAndRotation(transform.position, transform.rotation);
            projectile.Launch(target);
        }
    }
}