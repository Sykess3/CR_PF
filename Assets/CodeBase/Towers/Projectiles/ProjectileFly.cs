using UnityEngine;

namespace CodeBase.Towers.Projectiles
{
    public class ProjectileFly : MonoBehaviour
    {
        [SerializeField] private float _speed;
        private TrajectoryData _trajectoryData;
        private Transform _target;
        private Vector3 _startPosition;
        private Vector3 _linearFlyPosition;
        private float _trajectoryOffset;

        private void Update()
        {
            CalculateLinearMovement();
            AddTrajectoryOffset();
        }

        public void FlyTo(Transform target, TrajectoryData trajectoryData)
        {
            _target = target;
            _trajectoryData = trajectoryData;
            _startPosition = transform.position;
            _linearFlyPosition = transform.position;
            _trajectoryOffset = 0f;
        }

        private void CalculateLinearMovement() => 
            _linearFlyPosition = Vector3.MoveTowards(_linearFlyPosition, _target.position, Time.deltaTime * _speed);

        private void AddTrajectoryOffset()
        {
            float flyRatio = MathfCustom.InverseLerp(_startPosition, _target.position, _linearFlyPosition);
            _trajectoryOffset += _trajectoryData.Trajectory.Evaluate(flyRatio) * _trajectoryData.TrajectoryOffsetCoefficient * Time.deltaTime;
            transform.position = new Vector3(_linearFlyPosition.x, _linearFlyPosition.y + _trajectoryOffset, _linearFlyPosition.z);
        }
    }
}