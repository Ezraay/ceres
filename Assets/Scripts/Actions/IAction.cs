namespace CardGame.Actions
{
    public interface IAction
    {
        public void Execute(Battle battle, Player player);
    }
}