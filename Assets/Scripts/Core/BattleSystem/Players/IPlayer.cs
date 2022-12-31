using Ceres.Core.BattleSystem.Slots;

namespace Ceres.Core.BattleSystem.Players
{
    public interface IPlayer
    {
        CardSlot Champion { get; }
        MultiCardSlot Pile { get; }
        MultiCardSlot Hand { get; }
        MultiCardSlot Graveyard { get; }
        MultiCardSlot Damage { get; }
        void PreGameSetup();
    }
}