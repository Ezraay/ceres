using Ceres.Core.BattleSystem.Slots;

namespace Ceres.Core.BattleSystem.Players
{
    public interface IPlayer
    {
        CardSlot Champion { get; }
        IMultiCardSlot Pile { get; }
        IMultiCardSlot Hand { get; }
        IMultiCardSlot Graveyard { get; }
        IMultiCardSlot Damage { get; }
        void PreGameSetup();
    }
}