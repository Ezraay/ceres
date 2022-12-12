namespace CardGame
{
    public class Battle
    {
        public Player PriorityPlayer => Player1Priority ? Player1 : Player2;
        public readonly Player Player1;
        public readonly Player Player2;
        public AttackManager AttackManager;
        public BattlePhaseManager Phase;
        public bool Player1Priority = true;

        public Battle(Player player1, Player player2)
        {
            Player1 = player1;
            Player2 = player2;

            Phase = new BattlePhaseManager();
            Phase.OnTurnEnd += () => Player1Priority = !Player1Priority;

            AttackManager = new AttackManager();

            SetupPlayer(player1);
            SetupPlayer(player2);

            Phase.OnPhaseEnter += phase =>
            {
                switch (phase)
                {
                    case BattlePhase.Stand:
                        Execute(new Alert(PriorityPlayer.Champion));
                        break;
                    case BattlePhase.Draw:
                        Execute(new DrawFromPile());
                        break;
                    case BattlePhase.Defend:
                        if (!AttackManager.ValidAttack)
                            Execute(new SetPhase(BattlePhase.Draw));
                        break;
                }
            };
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