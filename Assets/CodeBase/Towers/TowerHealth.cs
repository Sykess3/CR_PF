using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Towers
{
    public class TowerHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private float _health;

        public event Action<float> Hit;
        public event Action Died;

        public void TakeDamage(float amount)
        {
            _health -= amount;
            if (_health <= 0)
            {
                _health = 0;
                Died?.Invoke();
            }

            Hit?.Invoke(_health);
        }
    }
}