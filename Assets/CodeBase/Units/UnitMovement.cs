using System;
using System.Collections;
using CodeBase.Grid;
using CodeBase.Grid.PathFinding;
using UnityEngine;

namespace CodeBase.Units
{
    public class UnitMovement : MonoBehaviour
    {
        [SerializeField] private PathfindingMotor _motor;
        private Transform _target;

        public void Construct(Transform target)
        {
            _target = target;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) 
                _motor.MoveTo(_target);
        }
    }
}