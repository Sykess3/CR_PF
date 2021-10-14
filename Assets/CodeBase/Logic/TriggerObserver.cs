using System;
using UnityEngine;

namespace CodeBase.Logic
{
    [RequireComponent(typeof(SphereCollider))]
    public class TriggerObserver : MonoBehaviour
    {
        [SerializeField] private Color _gizmosColor;

        [SerializeField] private SphereCollider _collider;
        public event Action<Collider> Entered;
        public event Action<Collider> Exited;

        private void OnTriggerEnter(Collider other) => 
            Entered?.Invoke(other);

        private void OnTriggerExit(Collider other) => 
            Exited?.Invoke(other);

        private void OnDrawGizmos()
        {
            if (_collider != null)
            {
                Gizmos.color = _gizmosColor;
                Gizmos.DrawWireSphere(transform.position + _collider.center, _collider.radius);
                Gizmos.color = Color.white;
            }

        }
    }
}