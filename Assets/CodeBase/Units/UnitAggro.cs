using System;
using CodeBase.Grid;
using CodeBase.Grid.PathFinding;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Units
{
    public class UnitAggro : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private CharacterMotor _motor;
        [Space]
        [SerializeField] private float _radius;
        private PathRequestManager _pathRequestManager;
        private Transform[] _baseTowers;
        private LayerMask _attackableMask;
        private Collider[] _nearestEnemy;
        private GridPath _lesserPath;
        private int _callBackCount_OnFoundNearestBaseTower;

        public Transform Target { get; private set; }
        public event Action<GridPath> FoundedNearestEnemy;

        public float Radius => _radius;

        public int BaseTowersCount => _baseTowers.Length;

        public void Construct(PathRequestManager pathRequestManager, Transform[] baseTowers)
        {
            _pathRequestManager = pathRequestManager;
            _baseTowers = baseTowers;
        }

        private void Start()
        {
            _attackableMask =
                (1 << LayerMask.NameToLayer("BlueUnit"));

            _nearestEnemy = new Collider[1];
        }

        private void Update()
        {
            if (Target != null)
                if (IsMovingTowardsTarget())
                    return;

            FindNearestEnemy();
        }

        private void FindNearestEnemy()
        {
            Target = null;
            _nearestEnemy[0] = null;
            Physics.OverlapSphereNonAlloc(transform.position, Radius, _nearestEnemy, _attackableMask);
            if (_nearestEnemy[0] != null)
            {
                Target = _nearestEnemy[0].transform;
                CreatePathRequest(FoundedNearestEnemy, _nearestEnemy[0].transform);
            }
        }

        private void FindNearestBaseTower()
        {
            ResetCallbackFields();
            foreach (var tower in _baseTowers)
                CreatePathRequest(OnFoundNearestBaseTower, tower);
        }

        private void OnFoundNearestBaseTower(GridPath path)
        {
            _callBackCount_OnFoundNearestBaseTower++;
            if (path.LengthCost == 0)
                return;

            if (_lesserPath == null)
                _lesserPath = path;
            else if (_lesserPath.LengthCost > path.LengthCost)
                _lesserPath = path;

            if (_callBackCount_OnFoundNearestBaseTower == BaseTowersCount)
                FoundedNearestEnemy?.Invoke(_lesserPath);
        }

        private void ResetCallbackFields()
        {
            _lesserPath = null;
            _callBackCount_OnFoundNearestBaseTower = 0;
        }

        private void CreatePathRequest(Action<GridPath> callback, Transform target)
        {
            var pathRequest = new PathRequest(
                start: transform.position,
                end: target.position,
                callBack: callback,
                _motor.TurnDistance,
                _motor.StoppingDistance);

            _pathRequestManager.RequestPath(pathRequest, gameObject);
        }
        
        private bool IsMovingTowardsTarget() => 
            !_motor.ReachedStoppingDistance(Target.position) && _motor.CurrentSpeed > 0;
    }
}