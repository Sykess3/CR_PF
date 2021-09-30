using System.Collections;
using CodeBase.Infrastructure.AssetsManagement;
using CodeBase.Infrastructure.Services.Dispatcher;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private Game _game;

        private void Awake()
        {
            IDispatcher dispatcher = LoadDispatcherWithInstantiating();
            _game = new Game(this, dispatcher);
            _game.EnterBootstrap();
            
            DontDestroyOnLoad(((Dispatcher)dispatcher).gameObject);
            DontDestroyOnLoad(this);
        }

        private IDispatcher LoadDispatcherWithInstantiating()
        {
            Dispatcher dispatcher = Resources.Load<Dispatcher>(AssetsPaths.Dispatcher);
            return Instantiate(dispatcher);
        }
    }
    
}
