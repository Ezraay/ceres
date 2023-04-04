using System;
using Ceres.Client;
using Ceres.Core.BattleSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace CardGame.BattleDisplay.HUD
{
	public class EndBattleScreen : MonoBehaviour
	{
		[SerializeField] private GameObject content;
		[SerializeField] private TMP_Text reasonText;
		private SceneManager sceneManager;

		[Inject]
		public void Construct(SceneManager scene)
		{
			this.sceneManager = scene;
		}
		
		private void Start()
		{
			Hide();
		}

		private void Hide()
		{
			this.content.SetActive(false);
		}

		public void Show(EndBattleReason reason)
		{
			this.reasonText.text = reason.ToString();
			
			this.content.SetActive(true);
		}

		public void LoadMainMenu()
		{
			this.sceneManager.LoadScene(new MainMenuScene());
		}
	}
}