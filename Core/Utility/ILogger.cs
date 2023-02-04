namespace Ceres.Core.Utility
{
    public interface ILogger
    {
        public void Log(object message);
        public void LogWarning(object message);
        public void LogError(object message);
    }
}