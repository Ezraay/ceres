using System;
using Ceres.Core.OldBattleSystem.Cards;

namespace Ceres.Core.OldBattleSystem.Slots
{
    public interface IMultiCardSlot : ISlot
    {
        public event Action<ICard> OnAdd;
        public event Action<ICard> OnRemove;
        public int Count { get; }
        void AddCard(ICard card);
        ICard GetCard(Guid id);
        void RemoveCard(ICard card);
        void Clear();
        bool Contains(ICard card);
        ICard PopCard();
    }
}