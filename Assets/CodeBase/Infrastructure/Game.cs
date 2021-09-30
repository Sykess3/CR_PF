using CodeBase.Infrastructure.GameStates;
using CodeBase.Infrastructure.Services.Dispatcher;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        private readonly IGameStateMachine _stateMachine;

        public Game(GameBootstrapper coroutineRunner, IDispatcher dispatcher)
        {
            _stateMachine = new GameStateMachine(
                new SceneLoader(coroutineRunner),
                coroutineRunner,
                new AllServices(),
                dispatcher);
        }

        public void EnterBootstrap() => 
            _stateMachine.Enter<ProjectBootstrapState>();
    }
}