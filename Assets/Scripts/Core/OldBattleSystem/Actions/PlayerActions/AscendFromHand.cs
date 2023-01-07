﻿using System;
using Ceres.Core.BattleSystem;
using Ceres.Core.OldBattleSystem.Battles;
using Ceres.Core.OldBattleSystem.Cards;
using Ceres.Core.OldBattleSystem.Players;

namespace Ceres.Core.OldBattleSystem.Actions.PlayerActions
{
    public class AscendFromHand : IAction
    {
        private readonly Guid cardID;

        public AscendFromHand(Guid cardID)
        {
            this.cardID = cardID;
        }

        public bool CanExecute(Battle battle, IPlayer player)
        {
            ICard card = player.Hand.GetCard(cardID);
            return battle.BattlePhaseManager.Phase == BattlePhase.Ascend &&
                   player.Hand.Contains(card) &&
                   player == battle.AttackingPlayer && (
                       card.Data.Tier == player.Champion.Card.Data.Tier ||
                       card.Data.Tier == player.Champion.Card.Data.Tier + 1
                   );
        }

        public void Execute(Battle battle, IPlayer player)
        {
            ICard card = player.Hand.GetCard(cardID);
            player.Champion.SetCard(card);
            player.Hand.RemoveCard(card);
            battle.BattlePhaseManager.Advance();
        }
    }
}