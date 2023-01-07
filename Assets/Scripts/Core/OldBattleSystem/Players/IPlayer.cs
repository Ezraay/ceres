using Ceres.Core.OldBattleSystem.Slots;

namespace Ceres.Core.OldBattleSystem.Players
{
    public interface IPlayer
    {
        CardSlot Champion { get; }
        IMultiCardSlot Pile { get; }
        IMultiCardSlot Hand { get; }
        IMultiCardSlot Graveyard { get; }
        IMultiCardSlot Damage { get; }
        void PreGameSetup();
        CardSlot GetCardSlot(int x, int y);
    }
}