using System;
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
        private BattleManager battleManager;
        private AdvancePhaseCommand command;

        [Inject]
        public void Construct(BattleManager battle)
        {
            battleManager = battle;
        }

        private void Start()
        {
            command = new AdvancePhaseCommand();
            battleManager.OnStartBattle += conditions =>
            {
                UpdatePhase(battleManager.Battle.PhaseManager.Phase);
                battleManager.Battle.PhaseManager.OnPhaseEnter += phase =>
                {
                    UpdatePhase(battleManager.Battle.PhaseManager.Phase);
                };
            };
        }

        public void NextPhase()
        {
            battleManager.Execute(command);
        }

        private void UpdatePhase(BattlePhase phase)
        {
            phaseText.text = phase.ToString();
            nextPhaseCommand.SetActive(command.CanExecute(battleManager.Battle, battleManager.MyPlayer));
        } 
    }
}