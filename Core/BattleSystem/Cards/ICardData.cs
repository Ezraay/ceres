﻿namespace Ceres.Core.BattleSystem
{
    public interface ICardData
    {
        public string ID { get; }
        public string Name { get; }
        public int Tier { get; }
        public int Attack { get; }
        public int Defense { get; }
    }
}