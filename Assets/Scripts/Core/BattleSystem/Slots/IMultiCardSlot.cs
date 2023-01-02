using System;
using Ceres.Core.BattleSystem.Cards;

namespace Ceres.Core.BattleSystem.Slots
{
    public interface IMultiCardSlot : ISlot
    {
        public event Action<ICard> OnAdd;
        public event Action<ICard> OnRemove;
        public int Count { get; }
        void AddCard(ICard card);
        void RemoveCard(ICard card);
        void Clear();
        bool Contains(ICard card);
        ICard PopCard();
    }
}