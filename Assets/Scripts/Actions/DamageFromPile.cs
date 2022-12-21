namespace CardGame
{
    public class DamageFromPile : IAction
    {
        public bool CanExecute(Battle battle, IPlayer player)
        {
            return player.Pile.Cards.Count > 0;
        }

        public void Execute(Battle battle, IPlayer player)
        {
            player.Damage.AddCard(player.Pile.PopCard());
        }
    }
}