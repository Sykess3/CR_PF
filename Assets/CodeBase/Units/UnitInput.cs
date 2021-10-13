using CodeBase.Grid;
using UnityEngine;

namespace CodeBase.Units
{
    public class UnitInput : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private UnitMovement _movement;
        [SerializeField] private UnitAggro _aggro;
        private GridPath _lesserPath;
        private int _callBackCount_OnFoundNearestBaseTower;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _aggro.FindNearestEnemy(OnFoundNearestEnemy);
            }
        }

        private void OnFoundNearestEnemy(GridPath path)
        {
            if (path.Length == 0)
            {
                ResetCallbackFields();
                _aggro.FindNearestBaseTower(OnFoundNearestBaseTower);
                return;
            }
            
            _movement.MoveAcross(path);
        }

        private void OnFoundNearestBaseTower(GridPath path)
        {
            _callBackCount_OnFoundNearestBaseTower++;
            if (path.LengthCost == 0)
                return;

            if (_lesserPath == null)
                _lesserPath = path;
            else if (_lesserPath.LengthCost > path.LengthCost)
                _lesserPath = path;

            if (_callBackCount_OnFoundNearestBaseTower == _aggro.BaseTowersCount) 
                _movement.MoveAcross(_lesserPath);
        }

        private void ResetCallbackFields()
        {
            _lesserPath = null;
            _callBackCount_OnFoundNearestBaseTower = 0;
        }
    }
}