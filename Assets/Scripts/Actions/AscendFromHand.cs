namespace CardGame.Actions
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
            return false; 
        }

        public void Execute(Battle battle, Player player)
        {
            player.Champion.SetCard(card);
            player.Hand.RemoveCard(card);
        }
    }
}