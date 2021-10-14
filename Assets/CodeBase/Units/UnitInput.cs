using System;
using CodeBase.Grid;
using UnityEngine;

namespace CodeBase.Units
{
    [SelectionBase]
    public class UnitInput : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private UnitMovement _movement;
        [SerializeField] private UnitAggro _aggro;

        private void OnEnable()
        {
            _aggro.FoundedNearestEnemy += OnFoundNearestEnemy;
        }

        private void OnDisable()
        {
            _aggro.FoundedNearestEnemy -= OnFoundNearestEnemy;
        }

        private void OnFoundNearestEnemy(GridPath path)
        {
            _movement.MoveAcross(path);
        }
        
    }
}