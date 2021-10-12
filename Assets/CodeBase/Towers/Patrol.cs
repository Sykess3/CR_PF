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
            _unitsMask = (1 << LayerMask.NameToLayer("Unit"));

            _arrowslit.enabled = false;
        }

        private void Update() => FindEnemyUnitsInRadius();

        private void FindEnemyUnitsInRadius()
        {
            Physics.OverlapSphereNonAlloc(transform.position, _radius, _detectedEnemyUnits, _unitsMask);
            if (_detectedEnemyUnits[0] != null)
            {
                _arrowslit.enabled = true;
                _arrowslit.Target = _detectedEnemyUnits[0].transform;
            }
        }
    }
}