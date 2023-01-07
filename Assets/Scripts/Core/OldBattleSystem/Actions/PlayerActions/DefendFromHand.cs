using System;
using Ceres.Core.BattleSystem;
using Ceres.Core.OldBattleSystem.Battles;
using Ceres.Core.OldBattleSystem.Cards;
using Ceres.Core.OldBattleSystem.Players;

namespace Ceres.Core.OldBattleSystem.Actions.PlayerActions
{
    public class DefendFromHand : IAction
    {
        private readonly Guid cardID;

        public DefendFromHand(Guid cardID)
        {
            this.cardID = cardID;
        }

        public bool CanExecute(Battle battle, IPlayer player)
        {
            ICard card = player.Hand.GetCard(cardID);
            return card != null &&
                   player.Hand.Contains(card) &&
                   battle.BattlePhaseManager.Phase == BattlePhase.Defend &&
                   player == battle.DefendingPlayer &&
                   card.Data.Tier <= player.Champion.Card.Data.Tier;
        }

        public void Execute(Battle battle, IPlayer player)
        {
            ICard card = player.Hand.GetCard(cardID);
            player.Hand.RemoveCard(card);
            battle.CombatManager.AddDefender(card);
        }
    }
}