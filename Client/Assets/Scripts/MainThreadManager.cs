using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Ceres.Client
{
    public class MainThreadManager : IFixedTickable
    {
        private static readonly List<Action> ExecuteOnMainThread = new();
        private static readonly List<Action> ExecuteCopiedOnMainThread = new();
        private static bool actionToExecuteOnMainThread;
        
        public void FixedTick()
        {
            if (!actionToExecuteOnMainThread) return;

            ExecuteCopiedOnMainThread.Clear();
            lock (ExecuteOnMainThread)
            {
                ExecuteCopiedOnMainThread.AddRange(ExecuteOnMainThread);
                ExecuteOnMainThread.Clear();
                actionToExecuteOnMainThread = false;
            }

            for (int i = 0; i < ExecuteCopiedOnMainThread.Count; i++) ExecuteCopiedOnMainThread[i]();
        }

        public void Execute(Action action)
        {
            if (action == null)
            {
                Debug.LogError("No action to execute on main thread!");
                return;
            }

            lock (ExecuteOnMainThread)
            {
                ExecuteOnMainThread.Add(action);
                actionToExecuteOnMainThread = true;
            }
        }
    }
}