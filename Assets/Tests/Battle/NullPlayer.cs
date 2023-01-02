using Ceres.Core.BattleSystem.Players;
using Ceres.Core.BattleSystem.Slots;

namespace Tests
{
    public class NullPlayer : IPlayer
    {
        public CardSlot Champion { get; }
        public IMultiCardSlot Pile { get; }
        public IMultiCardSlot Hand { get; }
        public IMultiCardSlot Graveyard { get; }
        public IMultiCardSlot Damage { get; }
        public void PreGameSetup()
        {
            
        }

        public CardSlot GetCardSlot(int x, int y)
        {
            return null;
        }
    }
}