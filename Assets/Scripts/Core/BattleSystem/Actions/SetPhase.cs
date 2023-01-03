using Ceres.Core.BattleSystem.Battles;
using Ceres.Core.BattleSystem.Players;

namespace Ceres.Core.BattleSystem.Actions
{
    public class SetPhase : IAction
    {
        private readonly BattlePhase phase;

        public SetPhase(BattlePhase phase)
        {
            this.phase = phase;
        }
        
        public bool CanExecute(Battle battle, IPlayer player)
        {
            return true;
        }

        public void Execute(Battle battle, IPlayer player)
        {
            battle.BattlePhaseManager.Set(phase);
        }
    }
}