using System;
using CodeBase.Grid;
using CodeBase.Grid.PathFinding;
using UnityEngine;

namespace CodeBase.Units
{
    public class UnitAggro : MonoBehaviour
    {
        [SerializeField] private CharacterMotor _motor;
        [SerializeField] private float _radius;
        private PathRequestManager _pathRequestManager;
        private Transform[] _baseTowers;
        private LayerMask _attackableMask;
        private Collider[] _nearestEnemy;

        public float Radius => _radius;

        public int BaseTowersCount => _baseTowers.Length;

        public void Construct(PathRequestManager pathRequestManager, Transform[] baseTowers)
        {
            _pathRequestManager = pathRequestManager;
            _baseTowers = baseTowers;
        }

        private void Start()
        {
            _attackableMask = 1 << LayerMask.NameToLayer("Attackable");
            _nearestEnemy = new Collider[1];
        }

        public void FindNearestBaseTower(Action<GridPath> callback)
        {
            foreach (var tower in _baseTowers)
            {
                var pathRequest = CreatePathRequest(callback, tower);

                _pathRequestManager.RequestPath(pathRequest, gameObject);
            }
        }

        public void FindNearestEnemy(Action<GridPath> callback)
        {
            Physics.OverlapSphereNonAlloc(transform.position, _radius, _nearestEnemy, _attackableMask);

            if (_nearestEnemy[0] == null)
            {
                callback.Invoke(EmptyPath());
                return;
            }

            var pathRequest = CreatePathRequest(callback, _nearestEnemy[0].transform);
            _pathRequestManager.RequestPath(pathRequest, gameObject);
        }

        private static GridPath EmptyPath()
        {
            var emptyArray = new Vector3[0];
            return new GridPath(
                emptyArray,
                Vector3.zero,
                0,
                0,
                0);
        }

        private PathRequest CreatePathRequest(Action<GridPath> callback, Transform target)
        {
            var pathRequest = new PathRequest(
                start: transform.position,
                end: target.position,
                callBack: callback,
                _motor.TurnDistance,
                _motor.StoppingDistance);
            return pathRequest;
        }
    }
}