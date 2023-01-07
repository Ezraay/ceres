using System;

namespace Ceres.Core.OldBattleSystem.Cards
{
    public class NullCard : ICard
    {
        public ICardData Data { get; }
        public Guid ID { get; } = Guid.Empty;
    }
}