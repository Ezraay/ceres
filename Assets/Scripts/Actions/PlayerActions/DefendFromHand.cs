namespace CardGame
{
    public class DefendFromHand : IAction
    {
        private readonly Card card;

        public DefendFromHand(Card card)
        {
            this.card = card;
        }
        
        public bool CanExecute(Battle battle, Player player)
        {
            return battle.Phase == BattlePhase.Defend && player != battle.AttackingPlayer && card.Data.Tier <= player.Champion.Card.Data.Tier;
        }

        public void Execute(Battle battle, Player player)
        {
            player.Hand.RemoveCard(card);
            battle.CombatManager.AddDefender(card);
        }
    }
}