using CardGame.Actions;

namespace CardGame
{
    public class Battle
    {
        public Player PriorityPlayer => Player1Priority ? Player1 : Player2;
        public readonly Player Player1;
        public readonly Player Player2;
        public bool Player1Priority = true;
        public BattlePhaseManager Phase;

        public Battle(Player player1, Player player2)
        {
            Player1 = player1;
            Player2 = player2;

            Phase = new BattlePhaseManager();
            Phase.OnTurnEnd += () => Player1Priority = !Player1Priority;
            Phase.OnChange += phase =>
            {
                switch (phase)
                {
                    case BattlePhase.Draw:
                        Execute(new DrawFromPile());
                        break;
                }
            };

            SetupPlayer(player1);
            SetupPlayer(player2);
        }

        private void SetupPlayer(Player player)
        {
            // TODO: Move this to the Player class
            for (int i = 0; i < 6; i++) Execute(new DrawFromPile(), player);
        }

        public void Execute(IAction action, Player author)
        {
            if (action.CanExecute(this, author))
                action.Execute(this, author);
        }

        public void Execute(IAction action)
        {
            Execute(action, PriorityPlayer);
        }
    }
}