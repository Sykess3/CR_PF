using System;

namespace CodeBase.Infrastructure.Services.Dispatcher
{
    public interface IDispatcher : IService
    {
        void Invoke(Action action);
        
        void InvokePending();
    }
}