using System;

namespace Ceres.Core.OldBattleSystem.Cards
{
    public interface ICard
    {
        public ICardData Data { get; }
        public Guid ID { get; }
    }
}