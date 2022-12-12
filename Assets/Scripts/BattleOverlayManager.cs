﻿using CardGame.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class BattleOverlayManager : MonoBehaviour
    {
        [SerializeField] private BattleManager battleManager;
        [SerializeField] private Text phaseText;
        [SerializeField] private Color player1Colour = Color.red;
        [SerializeField] private Color player2Colour = Color.blue;

        private void Start()
        {
            UpdatePhaseText(battleManager.battle.Phase.Value, battleManager.battle.Player1Priority);
        }

        public void NextPhaseButton()
        {
            battleManager.battle.Execute(new AdvancePhase());
            UpdatePhaseText(battleManager.battle.Phase.Value, battleManager.battle.Player1Priority);
        }

        private void UpdatePhaseText(BattlePhase phase, bool player1Turn)
        {
            phaseText.text = phase.ToString();
            phaseText.color = player1Turn ? player1Colour : player2Colour;
        }
    }
}