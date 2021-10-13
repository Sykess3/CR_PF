using System;
using System.Collections.ObjectModel;
using CodeBase.Grid.PathFinding;
using UnityEngine;

namespace CodeBase.Grid
{
    public class GridPath
    {
        private readonly Vector3[] _lookPoints;
        private readonly float _turnDistance;
        private readonly Line[] _turnBoundaries;

        public ReadOnlyCollection<Vector3> LookPoints => Array.AsReadOnly(_lookPoints);

        public int Length => _turnBoundaries.Length;
        public int LastElementIndex => Length - 1;
        public int LengthCost { get; }

        public Line this[int i] => _turnBoundaries[i];

        public int StopDistanceIndex { get; }

        public GridPath(Vector3[] waypoints, Vector3 startPos, float turnDistance, float stopDistance,
            int lengthCost)
        {
            _lookPoints = waypoints;
            _turnDistance = turnDistance;
            _turnBoundaries = FindTurnBoundaries(startPos);
            StopDistanceIndex = FindStopDistanceIndex(stopDistance);
            LengthCost = lengthCost;
        }

        private Line[] FindTurnBoundaries(Vector3 startPos)
        {
            var turnBoundaries = new Line[_lookPoints.Length];
            int finishLineIndex = turnBoundaries.Length - 1;

            Vector2 previousPoint = startPos.AsVector2();
            for (int i = 0; i < _lookPoints.Length; i++)
            {
                Vector2 currentPoint = _lookPoints[i].AsVector2();
                Vector2 dirToCurrentPoint = (currentPoint - previousPoint).normalized;

                Vector2 turnBoundaryPoint =
                    (i == finishLineIndex)
                        ? currentPoint
                        : currentPoint - dirToCurrentPoint * _turnDistance;

                turnBoundaries[i] = new Line(turnBoundaryPoint, previousPoint - dirToCurrentPoint * _turnDistance);

                previousPoint = turnBoundaryPoint;
            }

            return turnBoundaries;
        }

        private int FindStopDistanceIndex(float stopDistance)
        {
            float distanceFromEndPoint = 0;
            for (int i = _lookPoints.Length - 1; i > 0; i--)
            {
                distanceFromEndPoint += Vector3.Distance(_lookPoints[i], _lookPoints[i - 1]);
                if (distanceFromEndPoint > stopDistance)
                    return i;
            }
            return 0;
        }

        public void DrawWithGizmos()
        {
            Gizmos.color = Color.black;
            foreach (Vector3 p in _lookPoints)
            {
                Gizmos.DrawCube(p + Vector3.up, Vector3.one);
            }

            Gizmos.color = Color.white;
            foreach (Line l in _turnBoundaries)
            {
                l.DrawWithGizmos(10);
            }
        }
    }
}