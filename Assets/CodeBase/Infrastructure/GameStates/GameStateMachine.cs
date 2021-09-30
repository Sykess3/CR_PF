using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Dispatcher;
using CodeBase.Units;


namespace CodeBase.Infrastructure.GameStates
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableGameState> _states;
        private IExitableGameState _currentState;
        public GameStateMachine(SceneLoader sceneLoader, GameBootstrapper coroutineRunner, AllServices services, IDispatcher dispatcher)
        {
            _states = new Dictionary<Type, IExitableGameState>()
            {
                [typeof(ProjectBootstrapState)] = new ProjectBootstrapState(
                    this,
                    sceneLoader,
                    coroutineRunner,
                    services,
                    dispatcher
                ),
                [typeof(LoadLevelState)] = new LoadLevelState(services),
                
                [typeof(GameLoopState)] = new GameLoopState(this)
                
            };
        }
        public void Enter<TState>() where TState : class, IGameState
        {
            IGameState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : 
            class, IPayloadedGameState<TPayload>
        {
            IPayloadedGameState<TPayload> state = ChangeState<TState>();
            state.Enter(payload);
        }

        public TState ChangeState<TState>() where TState : class, IExitableGameState
        {
            _currentState?.Exit();
            TState state = GetState<TState>();
            _currentState = state;
            return state;
        }

        public TState GetState<TState>() where TState : class, IExitableGameState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}