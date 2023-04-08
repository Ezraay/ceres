using Ceres.Client;
using UnityEngine;
using Zenject;

namespace CardGame.BattleDisplay.HUD
{
	public class BattlePauseScreen : MonoBehaviour
	{
		[SerializeField] private GameObject content;
		private bool isVisible;
		private SceneManager sceneManager;

		[Inject]
		public void Construct(SceneManager scene)
		{
			this.sceneManager = scene;
		}
		private void Hide()
		{
			this.content.SetActive(false);
			this.isVisible = false;
		}

		private void Show()
		{
			this.content.SetActive(true);
			this.isVisible = true;
		}
		
		public void Toggle()
		{
			if (this.isVisible)
				Hide();
			else
				Show();
		}

		public void LoadMainMenu()
		{
			this.sceneManager.LoadScene(new MainMenuScene());
		}
	}
}