using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem
{
    public class UnitSlot : Slot
    {
        public int X { get; }
        public int Y { get; }
        [JsonProperty] public Card Card { get; private set; }
        public bool Exhausted { get; private set; }

        public UnitSlot(int x, int y)
        {
            X = x;
            Y = y;
        }
        
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