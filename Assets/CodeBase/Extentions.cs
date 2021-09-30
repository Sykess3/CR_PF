using System;
using CodeBase.Grid;
using CodeBase.Grid.PathFinding;
using UnityEngine;

namespace CodeBase
{
    public static class Extentions
    {
        public static Vector3 Direction(this Vector3 from, Vector3 to) =>
            (to - @from).normalized;

        public static int Distance(this Node from, Node to)
        {
            int xDistance = Mathf.Abs(to.PositionOnBoard.x - from.PositionOnBoard.x);
            int yDistance = Mathf.Abs(to.PositionOnBoard.y - from.PositionOnBoard.y);

            return yDistance < xDistance
                ? CalculateHorizontalMovementCost(yDistance, xDistance)
                : CalculateVerticalMovementCost(xDistance, yDistance);
        }

        public static Vector2 AsVector2(this Vector3 v3) => 
            new Vector2(v3.x, v3.z);

        public static float SqrDistanceTo(this Vector3 from, Vector3 point) => 
            (point - @from).sqrMagnitude;

        public static int LastElementIndex(this Array array) =>
            array.Length - 1;

        private static int CalculateVerticalMovementCost(int xDistance, int yDistance) =>
            PathFinder.DiagonalCost * xDistance + PathFinder.HorizontalOrVerticalCost * (yDistance - xDistance);

        private static int CalculateHorizontalMovementCost(int yDistance, int xDistance) =>
            PathFinder.DiagonalCost * yDistance + PathFinder.HorizontalOrVerticalCost * (xDistance - yDistance);
    }
}