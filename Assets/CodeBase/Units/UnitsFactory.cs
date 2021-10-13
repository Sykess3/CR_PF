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
        private readonly Transform[] _baseTowers;

        public UnitsFactory(PathRequestManager pathRequestManager, IAssets assets, Transform[] baseTowers)
        {
            
            _pathRequestManager = pathRequestManager;
            _assets = assets;
            _baseTowers = baseTowers;
        }
        public GameObject CreateUnit(Vector3 at)
        {
            GameObject unitObject = _assets.Instantiate(AssetsPaths.Unit, at);
            unitObject.GetComponent<UnitAggro>().Construct(_pathRequestManager, _baseTowers);
            return unitObject;
        } 
    }
}