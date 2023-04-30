using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem
{
    public class UnitSlot : Slot
    {
        public readonly CardPosition Position;
        [JsonProperty] public Card? Card { get; private set; }
        public bool Exhausted { get; private set; }

        public UnitSlot(CardPosition position)
        {
            this.Position = position;
        }
        
        public void Exhaust()
        {
            Exhausted = true;
        }

        public void SetCard(Card? card)
        {
            Card = card;
        }

        public void Clear()
        {
            Card = null;
        }

        public void Alert()
        {
            Exhausted = false;
        }
    }
}