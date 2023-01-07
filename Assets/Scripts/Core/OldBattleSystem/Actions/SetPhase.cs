using Ceres.Core.BattleSystem;
using Ceres.Core.OldBattleSystem.Battles;
using Ceres.Core.OldBattleSystem.Players;

namespace Ceres.Core.OldBattleSystem.Actions
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