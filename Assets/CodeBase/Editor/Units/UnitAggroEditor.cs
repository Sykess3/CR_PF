using CodeBase.Units;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor.Units
{
    public class UnitAggroEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(UnitAggro unitAggro, GizmoType gizmo)
        {
            Gizmos.DrawWireSphere(unitAggro.gameObject.transform.position, unitAggro.Radius);
        }
    }
}