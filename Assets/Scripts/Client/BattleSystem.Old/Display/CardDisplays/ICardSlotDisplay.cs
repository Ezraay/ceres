using Ceres.Core.OldBattleSystem.Players;
using Ceres.Core.OldBattleSystem.Slots;

namespace Ceres.Client.BattleSystem.Old.Display.CardDisplays
{
    public interface ICardSlotDisplay
    {
        public ISlot Slot { get; }
        public IPlayer Owner { get; }
    }
}