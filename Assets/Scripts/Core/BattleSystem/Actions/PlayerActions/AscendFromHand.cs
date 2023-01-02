using Ceres.Core.BattleSystem.Battles;
using Ceres.Core.BattleSystem.Cards;
using Ceres.Core.BattleSystem.Players;

namespace Ceres.Core.BattleSystem.Actions.PlayerActions
{
    public class AscendFromHand : IAction
    {
        private readonly ICard card;

        public AscendFromHand(ICard card)
        {
            this.card = card;
        }

        public bool CanExecute(Battle battle, IPlayer player)
        {
            return battle.BattlePhaseManager.Phase == BattlePhase.Ascend && 
                   player.Hand.Contains(card) && 
                   player == battle.AttackingPlayer && (
                card.Data.Tier == player.Champion.Card.Data.Tier || 
                card.Data.Tier == player.Champion.Card.Data.Tier + 1
                ); 
        }

        public void Execute(Battle battle, IPlayer player)
        {
            player.Champion.SetCard(card);
            player.Hand.RemoveCard(card);
            battle.BattlePhaseManager.Advance();
        }
    }
}