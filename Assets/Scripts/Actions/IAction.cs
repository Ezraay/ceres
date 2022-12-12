namespace CardGame.Actions
{
    public interface IAction
    {
        public bool CanExecute(Battle battle, Player player);
        
        public void Execute(Battle battle, Player player);
    }
}