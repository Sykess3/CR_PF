using CodeBase.Grid;
using CodeBase.Infrastructure.AssetsManagement;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    public class EnvironmentFactory : IService
    {
        private readonly AllServices _services;
        private readonly IAssets _assets;

        public EnvironmentFactory(AllServices services)
        {
            _services = services;
            _assets = services.Single<IAssets>();
        }
        public GameObject CreateGrid()
        {
            GameObject gridObject = _assets.Instantiate(AssetsPaths.Grid);
            gridObject.transform.localScale *= 4;
            gridObject.GetComponent<PlaneGrid>();
            return gridObject;
        }

        public GameObject CreateObstacle(Vector3 position, Vector3 size)
        {
            GameObject obstacleObject = _assets.Instantiate(AssetsPaths.Obstacle);
            obstacleObject.transform.position = position;
            obstacleObject.transform.localScale = size;
            return obstacleObject;
        }
    }
}