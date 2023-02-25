namespace Ceres.Core.BattleSystem
{
    public class UnitSlot
    {
        public Card Card { get; private set; }
        public bool Exhausted { get; private set; }
        
        public void Exhaust()
        {
            Exhausted = true;
        }

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