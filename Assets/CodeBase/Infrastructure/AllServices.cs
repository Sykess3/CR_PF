using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class AllServices
    {
        
        public void RegisterSingle<TService>(TService implementation) where TService : IService =>
            Implementation<TService>.ServiceInstance = implementation;

        public TService Single<TService>() where TService : IService =>
            Implementation<TService>.ServiceInstance;


        private static class Implementation<TService> where TService : IService
        {
            public static TService ServiceInstance;
        }
    }
}