using System;
using System.Collections;
using CodeBase.Grid;
using CodeBase.Grid.PathFinding;
using CodeBase.Grid.PathFinding.Threading;
using UnityEngine;

namespace CodeBase.Units
{
    [RequireComponent(typeof(PathfindingMotor))]
    public class UnitMovement : MonoBehaviour
    {
        [Header("Refs")] 
        [SerializeField] private UnitAnimator _animator;
        [SerializeField] private CharacterMotor _motor;
        private Transform _target;

        public void Construct(Transform target)
        {
            _target = target;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) 
                _motor.MoveTo(_target);
            
            _animator.Move(_motor.CurrentSpeed);
        }
    }
}