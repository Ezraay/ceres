using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Ceres.Client
{
    public class MainThreadManager : MonoBehaviour
    {
        private readonly List<Action> executeOnMainThread = new();
        private readonly List<Action> executeBuffer = new();
        private bool actionToExecuteOnMainThread;
        
        public void FixedUpdate()
        {
            if (!actionToExecuteOnMainThread) return;

            executeBuffer.Clear();
            lock (executeOnMainThread)
            {
                executeBuffer.AddRange(executeOnMainThread);
                executeOnMainThread.Clear();
                actionToExecuteOnMainThread = false;
            }

            for (int i = 0; i < executeBuffer.Count; i++) executeBuffer[i]();
        }

        public void Execute(Action action)
        {
            if (action == null)
            {
                Debug.LogError("No action to execute on main thread!");
                return;
            }

            lock (executeOnMainThread)
            {
                executeOnMainThread.Add(action);
                actionToExecuteOnMainThread = true;
            }
        }
    }
}