namespace Ceres.Core.BattleSystem
{
    public class ClientBattle
    {
        public IPlayer AllyPlayer { get; }
        public IPlayer OpponentPlayer { get; }
        public BattlePhaseManager PhaseManager { get; } = new BattlePhaseManager();
        private bool myTurn;

        public ClientBattle(IPlayer ally, IPlayer opponent, bool myTurn)
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