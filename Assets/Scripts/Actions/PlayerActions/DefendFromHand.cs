using UnityEngine;

namespace CardGame
{
    public class DefendFromHand : IAction
    {
        private readonly ICard card;

        public DefendFromHand(ICard card)
        {
            this.card = card;
        }
        
        public bool CanExecute(Battle battle, IPlayer player)
        {
            return card != null && 
                   player.Hand.Cards.Contains(card) &&
                   battle.BattlePhaseManager.Phase == BattlePhase.Defend && 
                   player == battle.DefendingPlayer && 
                   card.Data.Tier <= player.Champion.Card.Data.Tier;
        }

        public void Execute(Battle battle, IPlayer player)
        {
            player.Hand.RemoveCard(card);
            battle.CombatManager.AddDefender(card);
        }
    }
}