using CodeBase.Infrastructure.GameStates;

namespace CodeBase.Infrastructure
{
    public class GameLoopState : IGameState
    {
        private readonly IGameStateMachine _stateMachine;

        public GameLoopState(IGameStateMachine gameStateMachine)
        {
            _stateMachine = gameStateMachine;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }
}