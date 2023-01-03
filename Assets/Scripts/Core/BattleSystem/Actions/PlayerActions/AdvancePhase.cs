using Ceres.Core.BattleSystem.Battles;
using Ceres.Core.BattleSystem.Players;

namespace Ceres.Core.BattleSystem.Actions.PlayerActions
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