using System;
using CodeBase.Grid;
using CodeBase.Markers;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor.Markers
{
    public class GridMarkerEditor : UnityEditor.Editor
    {
        private static Vector3 _nodeSize;

        private void OnEnable()
        {
            _nodeSize = new Vector3(Node.Diameter, 0, Node.Diameter) * 0.9f;
        }

        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(GridMarker grid, GizmoType gizmo)
        {
            Gizmos.color = Color.green;
            var gridSize = grid.transform.localScale * 10f;

            Gizmos.DrawWireCube(grid.transform.position, gridSize);
            
            var gridSizeWithWorldSpaceCenter =
                new Vector3Int(
                    Mathf.FloorToInt(-gridSize.x * 0.5f),
                    Mathf.FloorToInt(gridSize.y),
                    Mathf.FloorToInt(-gridSize.z * 0.5f));

            for (int i = gridSizeWithWorldSpaceCenter.x; i < gridSize.x - gridSize.x * 0.5f; i++)
            {
                for (int j = gridSizeWithWorldSpaceCenter.z; j < gridSize.z - gridSize.z * 0.5f; j++)
                {
                    Gizmos.DrawCube(new Vector3(i + Node.Radius, 0, j + Node.Radius), new Vector3(Node.Diameter, 0, Node.Diameter) * 0.9f);
                }
            }
            Gizmos.color = Color.white;
        }
    }
}