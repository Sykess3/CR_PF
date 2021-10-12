using UnityEngine;

namespace CodeBase.Units
{
    [RequireComponent(typeof(Animator))]
    public class UnitAnimator : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Animator _animator;
        
        private static readonly int Defend = Animator.StringToHash("Defend");
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Attack01 = Animator.StringToHash("Attack_01");
        private static readonly int Attack02 = Animator.StringToHash("Attack_02");
        private static readonly int Velocity = Animator.StringToHash("Velocity");

        public void PlayDie() =>
            _animator.SetTrigger(Die);
        
        public void PlayHit()
        {
            StopDefend();
            _animator.SetTrigger(Hit);
        }

        public void PlayAttack_01()
        {
            StopDefend();
            _animator.SetTrigger(Attack01);
        }

        public void PlayAttack_02()
        {
            StopDefend();
            _animator.SetTrigger(Attack02);
        }

        public void PlayDefend() =>
            _animator.SetBool(Defend, true);
        
        public void Move(float speed)
        {
            StopDefend();
            _animator.SetFloat(Velocity, speed, 0.1f, Time.deltaTime);
        }

        private void StopDefend() => 
            _animator.SetBool(Defend, false);
    }
}