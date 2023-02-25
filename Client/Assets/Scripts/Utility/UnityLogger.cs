using UnityEngine;
using ILogger = Ceres.Core.Utility.ILogger;

namespace Ceres.Client.Utility
{
    public class UnityLogger : ILogger
    {
        public void Log(object message)
        {
            Debug.Log(message);
        }

        public void LogWarning(object message)
        {
            Debug.LogWarning(message);
        }

        public void LogError(object message)
        {
            Debug.LogError(message);
        }
    }
}