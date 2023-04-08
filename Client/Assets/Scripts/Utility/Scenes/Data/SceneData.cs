using System;

namespace Ceres.Client
{
	public abstract class SceneData
	{
		public abstract string SceneName { get; }
		
		public event Action OnSceneLeave;
		
		public virtual void SceneCleanup()
		{
			OnSceneLeave?.Invoke();
		}
	}
}