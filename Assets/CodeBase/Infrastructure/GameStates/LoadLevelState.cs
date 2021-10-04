using CodeBase.Grid;
using CodeBase.Grid.PathFinding;
using CodeBase.Grid.PathFinding.Threading;
using CodeBase.Infrastructure.AssetsManagement;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Dispatcher;
using CodeBase.Units;
using UnityEngine;

namespace CodeBase.Infrastructure.GameStates
{
    public class LoadLevelState : IPayloadedGameState<string>
    {
        private readonly AllServices _services;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly EnvironmentFactory _environmentFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly IAssets _assets;
        
        private  UnitsFactory _unitsFactory;

        public LoadLevelState(AllServices services)
        {
            _services = services;
            _gameStateMachine = _services.Single<IGameStateMachine>();
            _sceneLoader = _services.Single<ISceneLoader>();
            _environmentFactory = _services.Single<EnvironmentFactory>();
            _staticDataService = _services.Single<IStaticDataService>();
            _assets = _services.Single<IAssets>();
        }


        public void Enter(string payLoaded)
        {
            _sceneLoader.Load(name: payLoaded, OnLoaded);
        }

        public void Exit()
        {
        }

        private void OnLoaded()
        {
            PlaneGrid grid = CreatePathGrid();
            PathRequestManager pathRequestManager = CreatePathRequestService(grid);
            
            CreateFactories(pathRequestManager);
            Transform target = CreateTarget();
            CreateUnits(target);

            _gameStateMachine.Enter<GameLoopState>();
        }

        private void CreateUnitFactory(PathRequestManager pathRequestManager)
        {
            _unitsFactory = new UnitsFactory(pathRequestManager, _assets);
        }

        private PathRequestManager CreatePathRequestService(PlaneGrid grid)
        {
            PathGenerator pathGenerator = new PathGenerator_DotNetThreading(_services.Single<IDispatcher>());
            return new PathRequestManager(grid, pathGenerator);
        }

        private void CreateFactories(PathRequestManager pathRequestManager)
        {
            CreateUnitFactory(pathRequestManager);
        }

        private PlaneGrid CreatePathGrid() => 
            _environmentFactory.CreateGrid().GetComponent<PlaneGrid>();

        private Transform CreateTarget()
        {
            var targetObject = Resources.Load<GameObject>(AssetsPaths.Target);

            return Object.Instantiate(targetObject, new Vector3(1.9f, 0f, -6.94f), Quaternion.identity).transform;
        }

        private void CreateUnits(Transform target)
        {
            _unitsFactory.CreateUnit(new Vector3(14.5f, 0, 14.5f), target);
            // _unitsFactory.CreateUnit(new Vector3(-16.5f, 0, 8.5f), target);
            // _unitsFactory.CreateUnit(new Vector3(14.5f, 0, -14.5f), target);
            // _unitsFactory.CreateUnit(new Vector3(-16.5f, 0, -14.5f), target);
        }
    }
}