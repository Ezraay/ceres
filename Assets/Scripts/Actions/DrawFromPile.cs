namespace CardGame
{
    public class DrawFromPile : IAction
    {
        public bool CanExecute(Battle battle, Player player)
        {
            return player.Pile.Cards.Count > 0;
        }

        public void Execute(Battle battle, Player player)
        {
            player.Hand.AddCard(player.Pile.PopCard());
        }
    }
}