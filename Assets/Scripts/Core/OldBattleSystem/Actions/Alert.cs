using Ceres.Core.OldBattleSystem.Battles;
using Ceres.Core.OldBattleSystem.Players;
using Ceres.Core.OldBattleSystem.Slots;

namespace Ceres.Core.OldBattleSystem.Actions
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