using System.Collections;
using CodeBase.Grid;
using CodeBase.Grid.PathFinding;
using UnityEngine;

namespace CodeBase.Units
{
    public class UnitMotor : CharacterMotor
    {
        private GridPath _currentPath;
        private Coroutine _currentCoroutine;
        private WaitForFixedUpdate _waitForFixedUpdate;
        private float _speedFactor;
        private bool _needDecreaseMovementSpeed;

        private void Start() => _waitForFixedUpdate = new WaitForFixedUpdate();

        public override void MoveAcross(GridPath path)
        {
            if (path.Length == 0)
                return;

            _currentPath = path;
            if (_currentCoroutine != null) 
                StopCoroutine(_currentCoroutine);

            _currentCoroutine = StartCoroutine(StartMovement(_currentPath));
        }
        
        private IEnumerator StartMovement(GridPath path)
        {
            MovementStarted();

            int waypointIndex = 0;
            Line turnBoundary = path[0];
            transform.LookAt(path.LookPoints[0]);

            var mustMove = true;
            while (mustMove)
            {
                if (turnBoundary.HasCrossedLine(transform.position.AsVector2()))
                {
                    if (waypointIndex < path.LastElementIndex)
                    {
                        waypointIndex++;
                        turnBoundary = path[waypointIndex];
                    }
                }
                MoveTowards(path.LookPoints[waypointIndex]);

                mustMove = !CrossedStoppingDistance(waypointIndex);
                _waitForFixedUpdate = new WaitForFixedUpdate();
                yield return _waitForFixedUpdate;
            }
        }


        public override bool ReachedStoppingDistance(Vector3 position) => 
            transform.position.SqrDistanceTo(position) < _stoppingDistance * _stoppingDistance;

        private bool CrossedStoppingDistance(int waypointIndex)
        {
            if (_currentPath.StopDistanceIndex <= waypointIndex)
            {
                DecreaseCurrentSpeed();
                if (ReachedStoppingDistance(_currentPath.LookPoints[waypointIndex]))
                {
                    CurrentSpeed = 0;
                    return true;
                }
            }

            return false;
        }

        private void MoveTowards(Vector3 currentWaypoint)
        {
            CurrentSpeed = _movementSpeed * SpeedFactor(currentWaypoint);
            Quaternion targetRotation = Quaternion.LookRotation(currentWaypoint - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _angularSpeed);
            transform.Translate(Vector3.forward * (CurrentSpeed * Time.deltaTime), Space.Self);
        }

        private float SpeedFactor(Vector3 currentWaypoint)
        {
            if (_needDecreaseMovementSpeed)
                _speedFactor -= Time.deltaTime * 2 - DistanceFactor(currentWaypoint);
            else
                _speedFactor += Time.deltaTime; 
            
            _speedFactor = Mathf.Clamp(_speedFactor, 0f, 1f);
            return _speedFactor;
        }

        private float DistanceFactor(Vector3 currentWaypoint)
        {
            return (currentWaypoint - transform.position).sqrMagnitude / 10;
        }

        private void MovementStarted()
        {
            
            _speedFactor = 0f;
            _needDecreaseMovementSpeed = false;
        }

        private void DecreaseCurrentSpeed()
        {
            _needDecreaseMovementSpeed = true;
        }
    }
}