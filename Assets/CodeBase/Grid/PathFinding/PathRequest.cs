using System;
using UnityEngine;

namespace CodeBase.Grid.PathFinding
{
    public readonly struct PathRequest
    {
        public float StoppingDistance { get; }
        public int TurnDistance { get; }
        public Vector3 Start { get; }
        public Vector3 End { get; }
        public Action<GridPath> CallBack { get; }

        public PathRequest(Vector3 start, Vector3 end, Action<GridPath> callBack, int turnDistance,
            float stoppingDistance)
        {
            StoppingDistance = stoppingDistance;
            TurnDistance = turnDistance;
            CallBack = callBack;
            End = end;
            Start = start;
        }
    }
}