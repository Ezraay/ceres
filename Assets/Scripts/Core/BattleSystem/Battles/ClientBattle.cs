using Ceres.Core.BattleSystem.Battles;

namespace Ceres.Core.BattleSystem
{
    public class ClientBattle
    {
        private bool player1Turn;
        public AllyPlayer AllyPlayer { get; }
        public OpponentPlayer OpponentPlayer { get; }
        public BattlePhaseManager PhaseManager { get; } = new BattlePhaseManager();

        public ClientBattle(AllyPlayer ally, OpponentPlayer opponent)
        {
            AllyPlayer = ally;
            OpponentPlayer = opponent;
        }
        
        public bool IsPriorityPlayer()
        {
            switch (PhaseManager.Phase)
            {
                default:
                    return player1Turn;
            }
        }

        public void Apply(IServerAction action)
        {
            action.Apply(this);
        }
    }
}