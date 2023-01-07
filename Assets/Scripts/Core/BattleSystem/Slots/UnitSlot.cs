using Ceres.Core.BattleSystem.Cards;

namespace Ceres.Core.BattleSystem
{
    public class UnitSlot
    {
        public Card Card { get; private set; }
        
        public void SetCard(Card card)
        {
            Card = card;
        }

        public void Clear()
        {
            Card = null;
        }
    }
}