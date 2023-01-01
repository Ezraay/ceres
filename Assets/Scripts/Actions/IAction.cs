namespace CardGame
{
    public interface IAction
    {
        public bool CanExecute(Battle battle, IPlayer player);
        
        public void Execute(Battle battle, IPlayer player);
    }
}