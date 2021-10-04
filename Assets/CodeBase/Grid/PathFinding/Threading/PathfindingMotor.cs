using System.Collections;
using UnityEngine;

namespace CodeBase.Grid.PathFinding.Threading
{
    public class PathfindingMotor : CharacterMotor
    {
        private PathRequestManager _pathRequestManager;
        private GridPath _currentPath;
        private Coroutine _currentCoroutine;
        private WaitForFixedUpdate _waitForFixedUpdate;

        public override void Construct(PathRequestManager pathRequestManager)
        {
            _pathRequestManager = pathRequestManager;
            _waitForFixedUpdate = new WaitForFixedUpdate();
        }

        public override void MoveTo(Transform target)
        {
            var pathRequest = new PathRequest(
                start: transform.position,
                end: target.position,
                callBack: StartMovement,
                TurnDistance,
                StoppingDistance);

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
        

        private bool CrossedStoppingDistance(int waypointIndex)
        {
            if (_currentPath.StopDistanceIndex <= waypointIndex)
                if (ReachedStoppingDistance(_currentPath.LookPoints[waypointIndex]))
                    return true;

            return false;
        }

        private bool ReachedStoppingDistance(Vector3 position) => 
            transform.position.SqrDistanceTo(position) < StoppingDistance * StoppingDistance;

        private void MoveTowards(Vector3 currentWaypoint)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentWaypoint - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * AngularSpeed);
            transform.Translate(Vector3.forward * (Speed * Time.deltaTime), Space.Self);
        }

        private void OnDrawGizmos()
        {
            _currentPath?.DrawWithGizmos();
        }
    }
}