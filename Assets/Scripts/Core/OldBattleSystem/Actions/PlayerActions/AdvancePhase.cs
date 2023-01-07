using Ceres.Core.OldBattleSystem.Battles;
using Ceres.Core.OldBattleSystem.Players;

namespace Ceres.Core.OldBattleSystem.Actions.PlayerActions
{
    public class AdvancePhase : IAction
    {
        public bool CanExecute(Battle battle, IPlayer player)
        {
            return player == battle.PriorityPlayer;
        }

        public void Execute(Battle battle, IPlayer player)
        {
            battle.BattlePhaseManager.Advance();
        }
    }
}