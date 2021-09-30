using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.Dispatcher
{
    public class Dispatcher : MonoBehaviour, IDispatcher
    {
        private readonly List<Action> _pending = new List<Action>();

        private void Update() => InvokePending();

        public void Invoke(Action action)
        {
            lock (_pending)
                _pending.Add(action);
        }

        public void InvokePending()
        {
            lock (_pending)
            {
                foreach (Action action in _pending)
                    action.Invoke();

                _pending.Clear();
            }
        }
    }
}