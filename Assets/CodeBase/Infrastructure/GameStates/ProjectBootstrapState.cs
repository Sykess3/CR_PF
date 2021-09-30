using CodeBase.Infrastructure.AssetsManagement;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Dispatcher;
using UnityEngine;

namespace CodeBase.Infrastructure.GameStates
{
    public class ProjectBootstrapState : IGameState
    {
        private const string SampleScene = "SampleScene";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly GameBootstrapper _coroutineRunner;
        private readonly AllServices _services;
        private readonly IDispatcher _dispatcher;

        public ProjectBootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader,
            GameBootstrapper coroutineRunner, AllServices services, IDispatcher dispatcher)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _coroutineRunner = coroutineRunner;
            _services = services;
            _dispatcher = dispatcher;

            RegisterServices();
        }

        public void Enter()
        {
            RegisterServices();
            EnterLoadLevel();
        }

        public void Exit()
        {
        }

        private void EnterLoadLevel() =>
            _stateMachine.Enter<LoadLevelState, string>(SampleScene);

        private void RegisterServices()
        {
            GameStateMachine();
            CoroutineRunner();
            AssetsProvider();
            SceneLoader();
            StaticDataService();
            EnvironmentFactory();
            DispatcherService();
        }

        private void DispatcherService()
        {
            _services.RegisterSingle<IDispatcher>(_dispatcher);
        }


        private void CoroutineRunner()
        {
            _services.RegisterSingle<ICoroutineRunner>(_coroutineRunner);
        }

        private void SceneLoader()
        {
            _services.RegisterSingle<ISceneLoader>(_sceneLoader);
        }

        private void StaticDataService()
        {
            _services.RegisterSingle<IStaticDataService>(new StaticDataService());
            _services.Single<IStaticDataService>().Load();
        }

        private IAssets AssetsProvider()
        {
            _services.RegisterSingle<IAssets>(new AssetProvider());
            return _services.Single<IAssets>();
        }

        private void EnvironmentFactory()
        {
            _services.RegisterSingle(new EnvironmentFactory(_services));
        }


        private IGameStateMachine GameStateMachine()
        {
            _services.RegisterSingle<IGameStateMachine>(_stateMachine);
            return _services.Single<IGameStateMachine>();
        }
    }
}