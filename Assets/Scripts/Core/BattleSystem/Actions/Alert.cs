using Ceres.Core.BattleSystem.Battles;
using Ceres.Core.BattleSystem.Players;
using Ceres.Core.BattleSystem.Slots;

namespace Ceres.Core.BattleSystem.Actions
{
    public class Alert : IAction
    {
        private readonly int x;
        private readonly int y;

        public Alert(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        public bool CanExecute(Battle battle, IPlayer player)
        {
            CardSlot slot = player.GetCardSlot(x, y);
            return slot != null && 
                   slot.Card != null;
        }

        public void Execute(Battle battle, IPlayer player)
        {
            CardSlot slot = player.GetCardSlot(x, y);
            slot.Alert();
        }
    }
}