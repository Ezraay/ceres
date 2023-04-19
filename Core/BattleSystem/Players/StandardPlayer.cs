using System;
using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem
{
    public class StandardPlayer : IPlayer
    {
        [JsonProperty] private IMultiCardSlot Pile { get; }
        [JsonProperty] private IMultiCardSlot Hand { get; }
        [JsonProperty] private IMultiCardSlot Graveyard { get; } = new MultiCardSlot();
        [JsonProperty] private IMultiCardSlot Damage { get; } = new MultiCardSlot();
        [JsonProperty] private IMultiCardSlot Defense { get; } = new MultiCardSlot();
        [JsonIgnore] public UnitSlot Champion => units[1, 0];

        [JsonProperty] private readonly UnitSlot[,] units;

        public StandardPlayer(Guid id, IMultiCardSlot hand, IMultiCardSlot pile)
        {
            units = new UnitSlot[Width, Height];
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    units[x, y] = new UnitSlot(new CardPosition(x, y));

            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            
            Hand = hand;
            Pile = pile;
        }

        public Guid Id { get; }
        [JsonIgnore] public int Width => 3;
        [JsonIgnore] public int Height => 2;

        public void LoadDeck(IDeck deck)
        {
            foreach (ICardData cardData in deck.GetPile())
            {
                Pile.AddCard(new Card(cardData));
            }
            
            Champion.SetCard(new Card(deck.GetChampion()));
        }

        public UnitSlot GetUnitSlot(CardPosition position)
        {
            if (position.X < 0 || position.Y < 0 || position.X >= Width || position.Y >= Height)
                throw new Exception("Can't find slot with position: " + position);
            return units[position.X, position.Y];
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
                MultiCardSlotType.Pile => Pile,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}