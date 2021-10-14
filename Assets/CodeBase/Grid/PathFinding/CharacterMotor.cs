using CodeBase.Grid.PathFinding.Threading;
using UnityEngine;

namespace CodeBase.Grid.PathFinding
{
    public abstract class CharacterMotor : MonoBehaviour
    {
        [SerializeField] protected float _movementSpeed;
        [SerializeField] protected float _angularSpeed;
        [SerializeField] protected int _turnDistance;

        [Range(0.1f, 15f)]
        [SerializeField] protected float _stoppingDistance;

        public float MovementSpeed => _movementSpeed;

        public float AngularSpeed => _angularSpeed;

        public int TurnDistance => _turnDistance;
        public float StoppingDistance => _stoppingDistance;

        public float CurrentSpeed { get; protected set; }

        public abstract void MoveAcross(GridPath path);
        public abstract bool ReachedStoppingDistance(Vector3 position);
    }
}