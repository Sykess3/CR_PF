using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Units
{
    public class UnitHealth : MonoBehaviour, IHealth
    {
        [Header("Refs")]
        [SerializeField] private UnitAnimator _animator;
        [Space]
        [SerializeField]private float _health;

        public event Action<float> Hit; 
        public event Action Died; 
        
        public void TakeDamage(float amount)
        {
            _health -= amount;
            _animator.PlayHit();
            if (_health <= 0)
            {
                _health = 0;
                _animator.PlayDie();
                Died?.Invoke();
            }
            Hit?.Invoke(_health);
        }
    }
}