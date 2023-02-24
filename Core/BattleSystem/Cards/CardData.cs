using System;

namespace Ceres.Core.BattleSystem
{
    public class CardData : ICardData
    {
        public string ID { get; }
        public string Name { get; }
        public int Tier { get; }
        public int Attack { get; }
        public int Defense { get; }

        public CardData(string id, string name, int tier, int attack, int defense)
        {
            ID = id;
            Name = name;
            Tier = tier;
            Attack = attack;
            Defense = defense;
        }

        public override string ToString()
        {
            return ID;
        }
    }
}