using System;
using UnityEngine;

namespace CodeBase.Towers
{
    public class Patrol : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Arrowslit _arrowslit;
        [Space]
        [SerializeField] private float _radius;
        private Collider[] _detectedEnemyUnits;
        private LayerMask _unitsMask;

        public float Radius => _radius;

        private void Start()
        {
            _detectedEnemyUnits = new Collider[1];
            _unitsMask = (1 << LayerMask.NameToLayer("RedUnit"));
        }

        private void Update() => FindEnemyUnitsInRadius();

        private void FindEnemyUnitsInRadius()
        {
            Physics.OverlapSphereNonAlloc(transform.position, _radius, _detectedEnemyUnits, _unitsMask);
            if (_detectedEnemyUnits[0] != null)
            {
                _arrowslit.StartShoot();
                _arrowslit.Target = _detectedEnemyUnits[0].transform;
                return;
            }

            _arrowslit.StopShoot();
        }
    }
}