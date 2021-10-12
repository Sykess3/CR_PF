using CodeBase.Towers;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor.Towers
{
    public class PatrolEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(Patrol patrol, GizmoType gizmo)
        {
            Gizmos.DrawWireSphere(patrol.gameObject.transform.position, patrol.Radius);
        }
    }
}