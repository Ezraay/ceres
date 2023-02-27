using Ceres.Core.Entities;

namespace Ceres.Core.BattleSystem
{
    public class ServerPlayer 
    {
        public MultiCardSlot Hand { get; } = new MultiCardSlot();
        public MultiCardSlot Pile { get; } = new MultiCardSlot();
        public MultiCardSlot Graveyard { get; } = new MultiCardSlot();
        public MultiCardSlot Damage { get; } = new MultiCardSlot();
        public MultiCardSlot Defense { get; } = new MultiCardSlot();
        public UnitSlot Champion { get; } = new UnitSlot();
        public UnitSlot LeftUnit { get; } = new UnitSlot();
        public UnitSlot RightUnit { get; } = new UnitSlot();
        public UnitSlot LeftSupport { get; } = new UnitSlot();
        public UnitSlot RightSupport { get; } = new UnitSlot();
        public UnitSlot ChampionSupport { get; } = new UnitSlot();

        public void LoadDeck(IDeck deck)
        {
            foreach (ICardData cardData in deck.GetPile())
            {
                Pile.AddCard(new Card(cardData));
            }
            
            Champion.SetCard(new Card(deck.GetChampion()));
        }
    }
}