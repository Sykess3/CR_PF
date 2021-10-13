using System;
using System.Collections;
using CodeBase.Grid;
using CodeBase.Grid.PathFinding;
using CodeBase.Grid.PathFinding.Threading;
using UnityEngine;

namespace CodeBase.Units
{
    [RequireComponent(typeof(UnitMotor))]
    public class UnitMovement : MonoBehaviour
    {
        [Header("Refs")] 
        [SerializeField] private UnitAnimator _animator;
        [SerializeField] private CharacterMotor _motor;

        private void Update() => _animator.Move(_motor.CurrentSpeed);

        public void MoveAcross(GridPath path)
        {
            _motor.MoveAcross(path);
        }
    }
}