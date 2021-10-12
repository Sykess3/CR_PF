using System;
using CodeBase.Towers.Projectiles;
using UnityEngine;

namespace CodeBase.Towers
{
    [SelectionBase]
    public class Arrowslit : MonoBehaviour
    {
        [Header("Refs")] [SerializeField] private Transform _shootPoint;
        [Space] [SerializeField] private string _projectilesSceneName;
        [SerializeField] private float _delay;
        [SerializeField] private TrajectoryData _trajectoryData;
        [SerializeField] private Projectile _projectilePrefab;
        private ProjectilePool _projectilePool;

        private float _timeAfterShoot;
        private bool _canShoot;

        public Transform Target { get; set; }

        private void Start()
        {
            _projectilePool = new ProjectilePool(_projectilePrefab, _projectilesSceneName);
        }

        private void Update()
        {
            _timeAfterShoot += Time.deltaTime;
            if (!_canShoot)
                return;

            if (_timeAfterShoot >= _delay)
            {
                Shoot(Target);
                _timeAfterShoot = 0f;
            }
        }

        public void StartShoot() => _canShoot = true;

        public void StopShoot() => _canShoot = false;

        private void Shoot(Transform target)
        {
            Projectile projectile = _projectilePool.Get();
            projectile.transform.SetPositionAndRotation(_shootPoint.transform.position, transform.rotation);
            projectile.Launch(target, _trajectoryData);
        }
    }

    [Serializable]
    public class TrajectoryData
    {
        public AnimationCurve Trajectory;
        public float TrajectoryOffsetCoefficient;
    }
}