using Ceres.Core.Utility;

namespace Ceres.Client.Utility
{
    public static class Logger
    {
        private static readonly ILogger logger = new UnityLogger();

        public static void Log(object message)
        {
            logger.Log(message);
        }

        public static void LogWarning(object message)
        {
            logger.LogWarning(message);
        }

        public static void LogError(object message)
        {
            logger.LogError(message);
        }
    }
}