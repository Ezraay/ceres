namespace CardGame
{
    public class DrawFromPile : IAction
    {
        public bool CanExecute(Battle battle, IPlayer player)
        {
            return player.Pile.Cards.Count > 0;
        }

        public void Execute(Battle battle, IPlayer player)
        {
            player.Hand.AddCard(player.Pile.PopCard());
        }
    }
}