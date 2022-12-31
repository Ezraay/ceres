using Ceres.Core.BattleSystem.Players;
using Ceres.Core.BattleSystem.Slots;

namespace Tests
{
    public class NullPlayer : IPlayer
    {
        public CardSlot Champion { get; }
        public MultiCardSlot Pile { get; }
        public MultiCardSlot Hand { get; }
        public MultiCardSlot Graveyard { get; }
        public MultiCardSlot Damage { get; }
        public void PreGameSetup()
        {
            
        }
    }
}