using System;

namespace Ceres.Core.BattleSystem.Cards
{
    public interface ICard
    {
        public ICardData Data { get; }
        public Guid ID { get; }
    }
}