namespace CardGame
{
    public class DamageFromPile : IAction
    {
        public bool CanExecute(Battle battle, Player player)
        {
            return player.Pile.Cards.Count > 0;
        }

        public void Execute(Battle battle, Player player)
        {
            player.Damage.AddCard(player.Pile.PopCard());
        }
    }
}