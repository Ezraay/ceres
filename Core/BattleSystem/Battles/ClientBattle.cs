namespace Ceres.Core.BattleSystem
{
    public class ClientBattle
    {
        public AllyPlayer AllyPlayer { get; }
        public OpponentPlayer OpponentPlayer { get; }
        public BattlePhaseManager PhaseManager { get; } = new BattlePhaseManager();
        private bool myTurn;

        public ClientBattle(AllyPlayer ally, OpponentPlayer opponent, bool myTurn)
        {
            AllyPlayer = ally;
            OpponentPlayer = opponent;
            this.myTurn = myTurn;
        }

        public bool IsPriorityPlayer()
        {
            switch (PhaseManager.Phase)
            {
                default:
                    return myTurn;
            }
        }

        public void Apply(IServerAction action)
        {
            action.Apply(this);
        }
    }
}