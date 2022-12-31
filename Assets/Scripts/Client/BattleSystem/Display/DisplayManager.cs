﻿using Ceres.Client.BattleSystem.Display.CardDisplays;
using Ceres.Core.BattleSystem.Battles;
using UnityEngine;

namespace Ceres.Client.BattleSystem.Display
{
    public class DisplayManager : MonoBehaviour
    {
        [SerializeField] private PlayerDisplay player1Display;
        [SerializeField] private PlayerDisplay player2Display;
        [SerializeField] private MultiCardSlotDisplay defenderSlot;
        
        public void Setup(Battle battle)
        {
            player1Display.Setup(battle.Player1);
            player2Display.Setup(battle.Player2);
            defenderSlot.Setup(battle.CombatManager.Defenders, null);
        }
    }
}