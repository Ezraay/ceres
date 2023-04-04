using CardGame.BattleDisplay.HUD;
using CardGame.Networking;
using Ceres.Client.BattleSystem;
using Ceres.Core.BattleSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace CardGame.BattleDisplay
{
	public class BattleHUD : MonoBehaviour
	{
		[SerializeField] private GameObject nextPhaseCommand;
		[SerializeField] private TMP_Text phaseText;
		[SerializeField] private EndBattleScreen endBattleScreen;
		private BattleManager battleManager;
		private AdvancePhaseCommand command;

		private void OnEnable()
		{
			this.battleManager.OnStart += OnStart;
		}

		private void OnDisable()
		{
			this.battleManager.OnStart -= OnStart;
			if (this.battleManager.IsStarted)
			{
				this.battleManager.OnEnd -= ShowEnd;
				this.battleManager.Battle.PhaseManager.OnPhaseEnter -= UpdatePhase;
			}
		}

		[Inject]
		public void Construct(BattleManager battle)
		{
			this.battleManager = battle;
			this.command = new AdvancePhaseCommand();
		}

		private void OnStart(BattleStartConditions conditions)
		{
			UpdatePhase(this.battleManager.Battle.PhaseManager.Phase);
			this.battleManager.Battle.PhaseManager.OnPhaseEnter += UpdatePhase;
			this.battleManager.OnEnd += ShowEnd;
		}

		public void NextPhase()
		{
			this.battleManager.Execute(this.command);
		}

		private void UpdatePhase(BattlePhase phase)
		{
			this.phaseText.text = phase.ToString();
			Debug.Log(this.nextPhaseCommand);
			Debug.Log(this.command);
			Debug.Log(this.battleManager);
			this.nextPhaseCommand.SetActive(this.command.CanExecute(this.battleManager.Battle,
				this.battleManager.MyPlayer));
		}

		private void ShowEnd(EndBattleReason reason)
		{
			this.endBattleScreen.Show(reason);
		}
	}
}