using System;

namespace Ceres.Core.BattleSystem.Cards
{
    public class NullCard : ICard
    {
        public ICardData Data { get; }
        public Guid ID { get; } = Guid.Empty;
    }
}