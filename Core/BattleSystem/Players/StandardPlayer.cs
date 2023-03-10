using System;

namespace Ceres.Core.BattleSystem
{
    public class StandardPlayer : IPlayer
    {
        private IMultiCardSlot Pile { get; }
        private IMultiCardSlot Hand { get; }
        private IMultiCardSlot Graveyard { get; } = new MultiCardSlot();
        private IMultiCardSlot Damage { get; } = new MultiCardSlot();
        private IMultiCardSlot Defense { get; } = new MultiCardSlot();
        public UnitSlot Champion => Units[1, 0];

        private readonly UnitSlot[,] Units;

        public StandardPlayer(IMultiCardSlot hand, IMultiCardSlot pile)
        {
            Units = new UnitSlot[Width, Height];
            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                Units[x, y] = new UnitSlot(x, y);

            Hand = hand;
            Pile = pile;
        }

        public int Width => 3;
        public int Height => 2;

        public void LoadDeck(IDeck deck)
        {
            foreach (ICardData cardData in deck.GetPile())
            {
                Pile.AddCard(new Card(cardData));
            }
            
            Champion.SetCard(new Card(deck.GetChampion()));
        }

        public UnitSlot GetUnitSlot(int x, int y)
        {
            return Units[x, y];
        }

        public T GetMultiCardSlot<T>(MultiCardSlotType type) where T : IMultiCardSlot
        {
            return (T) GetMultiCardSlot(type);
        }

        public IMultiCardSlot GetMultiCardSlot(MultiCardSlotType type)
        {
            return type switch
            {
                MultiCardSlotType.Damage => Damage,
                MultiCardSlotType.Defense => Defense,
                MultiCardSlotType.Graveyard => Graveyard,
                MultiCardSlotType.Hand => Hand,
                MultiCardSlotType.Pile => Pile
            };
        }
    }
}