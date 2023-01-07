using Ceres.Core.OldBattleSystem.Battles;
using Ceres.Core.OldBattleSystem.Players;

namespace Ceres.Core.OldBattleSystem.Actions
{
    public interface IAction
    {
        public bool CanExecute(Battle battle, IPlayer player);
        
        public void Execute(Battle battle, IPlayer player);
    }
}