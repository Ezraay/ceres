namespace CardGame.Actions
{
    public class AscendAction : IAction
    {
        private readonly Card card;

        public AscendAction(Card card)
        {
            this.card = card;
        }
        
        public void Execute(Battle battle, Player player)
        {
            player.Champion.SetCard(card);
            player.Hand.RemoveCard(card);
        }
    }
}