﻿namespace Ceres.Core.BattleSystem.Cards
{
    public interface ICardData
    {
        public string Name { get; }
        public int Tier { get; }
        public int Attack { get; }
        public int Defense { get; }
    }
}