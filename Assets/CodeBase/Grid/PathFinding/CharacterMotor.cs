using CodeBase.Grid.PathFinding.Threading;
using UnityEngine;

namespace CodeBase.Grid.PathFinding
{
    public abstract class CharacterMotor : MonoBehaviour
    {
        [SerializeField] protected float Speed;
        [SerializeField] protected float AngularSpeed;
        [SerializeField] protected int TurnDistance;

        [Range(0.1f, 15f)]
        [SerializeField]
        protected float StoppingDistance;

        public virtual void Construct(PathRequestManager pathRequestManager) { }
        public abstract void MoveTo(Transform target);
    }
}