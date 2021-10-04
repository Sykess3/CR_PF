using CodeBase.Grid.PathFinding;
using CodeBase.Grid.PathFinding.Threading;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.AssetsManagement;
using CodeBase.Infrastructure.Factories;
using UnityEngine;

namespace CodeBase.Units
{
    public class UnitsFactory : IService
    {
        private readonly PathRequestManager _pathRequestManager;
        private readonly IAssets _assets;

        public UnitsFactory(PathRequestManager pathRequestManager, IAssets assets)
        {
            _pathRequestManager = pathRequestManager;
            _assets = assets;
        }
        public GameObject CreateUnit(Vector3 at, Transform target)
        {
            GameObject unitObject = _assets.Instantiate(AssetsPaths.Unit, at);
            unitObject.GetComponent<UnitMovement>().Construct(target);
            unitObject.GetComponent<CharacterMotor>().Construct(_pathRequestManager);
            return unitObject;
        } 
    }
}