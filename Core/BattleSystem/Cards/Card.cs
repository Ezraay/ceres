using System;
using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem
{
    public class Card
    {
        public ICardData Data { get; }
        public Guid ID { get; }

        public Card(ICardData data) : this(data, Guid.NewGuid()) { }

        [JsonConstructor]
        public Card(ICardData data, Guid id)
        {
            Data = data;
            ID = id;
        }

        public static Card TestCard()
        {
            return new Card(new CardData("archer", "Archer", 1, 5, 5));
        }
    }
}