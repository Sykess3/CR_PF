using UnityEngine;

namespace CodeBase.Grid.PathFinding
{
    public struct Line
    {
        private const float verticalLineGradient = 1e5f;

        private readonly float _gradient;
        private readonly Vector2 _pointOnLine_1;
        private readonly Vector2 _pointOnLine_2;
        private readonly bool _approachSide;


        public Line(Vector2 pointOnLine, Vector2 pointPerpendicularToLine)
        {
            float dx = pointOnLine.x - pointPerpendicularToLine.x;
            float dy = pointOnLine.y - pointPerpendicularToLine.y;

            if (dx == 0)
                _gradient = verticalLineGradient;
            else
                _gradient = -dx / dy;
            
            _pointOnLine_1 = pointOnLine;
            _pointOnLine_2 = pointOnLine + new Vector2(1, _gradient);

            _approachSide = false;
            _approachSide = GetSide(pointPerpendicularToLine);
        }

        public bool HasCrossedLine(Vector2 p) => 
            GetSide(p) != _approachSide;

        public void DrawWithGizmos(float length)
        {
            Vector3 lineDir = new Vector3(1, 0, _gradient).normalized;
            Vector3 lineCentre = new Vector3(_pointOnLine_1.x, 0, _pointOnLine_1.y) + Vector3.up;
            Gizmos.DrawLine(lineCentre - lineDir * length / 2f, lineCentre + lineDir * length / 2f);
        }

        private bool GetSide(Vector2 p) =>
            (p.x - _pointOnLine_1.x) * (_pointOnLine_2.y - _pointOnLine_1.y) >
            (p.y - _pointOnLine_1.y) * (_pointOnLine_2.x - _pointOnLine_1.x);
    }
}