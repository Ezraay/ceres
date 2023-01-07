using System;
using Ceres.Core.BattleSystem;
using Ceres.Core.BattleSystem.Battles;
using UnityEngine;
using UnityEngine.UI;

namespace Ceres.Client.BattleSystem
{
    public class BattlePhaseDisplay : MonoBehaviour
    {
        [SerializeField] private BattleManager battleManager;
        [SerializeField] private Text phaseText;

        private void Start()
        {
            battleManager.Battle.PhaseManager.OnPhaseEnter += OnPhaseEnter;
        }

        private void OnPhaseEnter(BattlePhase phase)
        {
            phaseText.text = phase.ToString();
        }

        public void NextPhaseButton()
        {
            battleManager.Execute(new AdvancePhaseCommand());
        }
    }
}