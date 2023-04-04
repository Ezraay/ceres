using System.Threading.Tasks;
using CardGame.BattleDisplay.Networking;
using Ceres.Core.BattleSystem;
using UnityEngine;

namespace Ceres.Client
{
	public class SceneManager
	{
		public SceneData CurrentScene { get; private set; }

		public SceneManager(IDeck testingDeck)
		{
			switch (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
			{
				case "Battle":
					bool myTurn = Random.Range(0f, 1f) < 0.5f; // In this case, local player is player 1
					ServerBattleStartConfig config = new ServerBattleStartConfig(testingDeck, testingDeck, myTurn);
					SinglePlayerProcessor processor = new SinglePlayerProcessor(config);
					this.CurrentScene = new BattleScene(processor);
					Debug.Log("Creating single-player test game");
					break;
			}
		}

		public async Task LoadSceneAsync(SceneData scene)
		{
			PreLoad(scene);
			AsyncOperation task = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene.SceneName);
			while (!task.isDone)
				await Task.Yield();
		}

		public void LoadScene(SceneData scene)
		{
			PreLoad(scene);
			UnityEngine.SceneManagement.SceneManager.LoadScene(scene.SceneName);
		}

		private void PreLoad(SceneData scene)
		{
			if (this.CurrentScene != null)
				this.CurrentScene.SceneCleanup();
			
			this.CurrentScene = scene;
			Debug.Log("Loading scene: " + scene.SceneName);
		}
	}
}