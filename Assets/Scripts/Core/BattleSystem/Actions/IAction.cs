using Ceres.Core.BattleSystem.Battles;
using Ceres.Core.BattleSystem.Players;

namespace Ceres.Core.BattleSystem.Actions
{
    public interface IAction
    {
        public bool CanExecute(Battle battle, IPlayer player);
        
        public void Execute(Battle battle, IPlayer player);
    }
}