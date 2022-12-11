namespace CardGame.Actions
{
    public class DrawAction : IAction
    {
        public void Execute(Battle battle, Player player)
        {
            player.Hand.AddCard(player.Pile.PopCard());
        }
    }
}