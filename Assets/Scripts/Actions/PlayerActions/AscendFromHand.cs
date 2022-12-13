namespace CardGame
{
    public class AscendFromHand : IAction
    {
        private readonly Card card;

        public AscendFromHand(Card card)
        {
            this.card = card;
        }

        public bool CanExecute(Battle battle, Player player)
        {
            return battle.Phase == BattlePhase.Ascend && (
                card.Data.Tier == player.Champion.Card.Data.Tier || 
                card.Data.Tier == player.Champion.Card.Data.Tier + 1
                ); 
        }

        public void Execute(Battle battle, Player player)
        {
            player.Champion.SetCard(card);
            player.Hand.RemoveCard(card);
            battle.Phase.Advance();
        }
    }
}