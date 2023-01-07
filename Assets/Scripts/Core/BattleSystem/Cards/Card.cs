using System;

namespace Ceres.Core.BattleSystem
{
    public class Card
    {
        public ICardData Data { get; }
        public Guid ID { get; }

        public Card(ICardData data) : this(data, Guid.NewGuid()) { }

        public Card(ICardData data, Guid id)
        {
            Data = data;
            ID = id;
        }
    }
}