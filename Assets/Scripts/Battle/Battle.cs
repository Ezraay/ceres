namespace CardGame
{
    public class Battle
    {
        public Player PriorityPlayer => Player1Priority ? Player1 : Player2;
        public Player AttackingPlayer => player1Turn ? Player1 : Player2;
        public Player DefendingPlayer => player1Turn ? Player2 : Player1;
        public bool Player1Priority { get; private set; } = true;
        public readonly CombatManager CombatManager;
        public readonly BattlePhaseManager Phase;
        public readonly Player Player1;
        public readonly Player Player2;
        private bool player1Turn = true;

        public Battle(Player player1, Player player2)
        {
            Player1 = player1;
            Player2 = player2;

            CombatManager = new CombatManager();

            Phase = new BattlePhaseManager();
            Phase.OnTurnEnd += () =>
            {
                player1Turn = !player1Turn;
                Player1Priority = player1Turn;
            };

            SetupPlayer(player1);
            SetupPlayer(player2);

            Phase.OnPhaseEnter += phase =>
            {
                switch (phase)
                {
                    case BattlePhase.Stand:
                        Execute(new Alert(AttackingPlayer.Champion));
                        break;
                    case BattlePhase.Draw:
                        Execute(new DrawFromPile());
                        break;
                    case BattlePhase.Defend:
                        if (!CombatManager.ValidAttack)
                            Execute(new SetPhase(BattlePhaseManager.LastPhase));
                        else
                            Player1Priority = !player1Turn;
                        break;
                    case BattlePhase.Damage:
                        Player1Priority = player1Turn;
                        for (int i = 0; i < CombatManager.DamageCount(); i++)
                        {
                            Execute(new DamageFromPile(), DefendingPlayer);
                        }
                        break;
                    case BattlePhase.End:
                        CombatManager.Reset(DefendingPlayer.Graveyard);
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