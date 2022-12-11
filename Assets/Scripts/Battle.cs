using CardGame.Actions;

namespace CardGame
{
    public class Battle
    {
        public readonly Player Player1;
        public readonly Player Player2;
        private bool player1Priority;
        public Player PriorityPlayer => player1Priority ? Player1 : Player2;

        public Battle(Player player1, Player player2)
        {
            this.Player1 = player1;
            this.Player2 = player2;
            
            Execute(new DrawAction(), player1);
            Execute(new DrawAction(), player1);

        }       
        
        public void Execute(IAction action, Player author)
        {
            action.Execute(this, author);
        }
    }
}