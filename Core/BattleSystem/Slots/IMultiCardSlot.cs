using System;

namespace Ceres.Core.BattleSystem
{
    public interface IMultiCardSlot
    {
        public void AddCard(Card card);
        public void RemoveCard(Card card);
        Card GetCard(Guid cardId);
    }
}