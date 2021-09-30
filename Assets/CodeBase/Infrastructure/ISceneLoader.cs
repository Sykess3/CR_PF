using System;

namespace CodeBase.Infrastructure
{
    public interface ISceneLoader : IService
    {
        void Load(string name,Action onLoaded = null);
    }
}