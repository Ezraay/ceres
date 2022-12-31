using Ceres.Core.BattleSystem.Players;
using Ceres.Core.BattleSystem.Slots;

namespace Ceres.Client.BattleSystem.Display.CardDisplays
{
    public interface ICardSlotDisplay
    {
        public ISlot Slot { get; }
        public IPlayer Owner { get; }
    }
}