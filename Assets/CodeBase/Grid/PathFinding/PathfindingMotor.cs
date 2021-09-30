using System;
using System.Collections;
using CodeBase.Infrastructure;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Grid.PathFinding
{
    public class PathfindingMotor : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _angularSpeed;
        [SerializeField] private int _turnDistance;
        [Range(0.1f, 15f)]
        [SerializeField] private float _stoppingDistance;

        private PathRequestManager _pathRequestManager;
        private GridPath _currentPath;
        private Coroutine _currentCoroutine;

        public void Construct(PathRequestManager pathRequestManager)
        {
            _pathRequestManager = pathRequestManager;
        }

        public void MoveTo(Transform target)
        {
            var pathRequest = new PathRequest(
                start: transform.position,
                end: target.position,
                callBack: StartMovement,
                _turnDistance,
                _stoppingDistance);

            _pathRequestManager.RequestPath(pathRequest, gameObject);
        }
        
        private void StartMovement(GridPath path)
        {
            if (path.Length == 0)
                return;

            _currentPath = path;
            if (_currentCoroutine != null) 
                StopCoroutine(_currentCoroutine);

            _currentCoroutine = StartCoroutine(MoveAcross(_currentPath));
        }
        
        private IEnumerator MoveAcross(GridPath path)
        {
            int waypointIndex = 0;
            Line turnBoundary = path[0];
            transform.LookAt(path.LookPoints[0]);

            var mustMove = true;
            while (mustMove)
            {
                if (turnBoundary.HasCrossedLine(transform.position.AsVector2()))
                {
                    waypointIndex++;
                    turnBoundary = path[waypointIndex];
                }
                MoveTowards(path.LookPoints[waypointIndex]);

                mustMove = !CrossedStoppingDistance(waypointIndex);
                yield return null;
            }
        }
        

        private bool CrossedStoppingDistance(int waypointIndex)
        {
            if (_currentPath.StopDistanceIndex == waypointIndex)
                if (ReachedStoppingDistance(_currentPath.LookPoints[waypointIndex]))
                    return true;

            return false;
        }

        private bool ReachedStoppingDistance(Vector3 position) => 
            transform.position.SqrDistanceTo(position) < _stoppingDistance * _stoppingDistance;

        private void MoveTowards(Vector3 currentWaypoint)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentWaypoint - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _angularSpeed);
            transform.Translate(Vector3.forward * (_speed * Time.deltaTime), Space.Self);
        }

        private void OnDrawGizmos()
        {
            _currentPath?.DrawWithGizmos();
        }
    }
}