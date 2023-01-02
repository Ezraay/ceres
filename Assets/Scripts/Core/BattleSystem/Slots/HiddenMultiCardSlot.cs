using System;
using Ceres.Core.BattleSystem.Cards;

namespace Ceres.Core.BattleSystem.Slots
{
    public class HiddenMultiCardSlot : IMultiCardSlot
    {
        public event Action<ICard> OnAdd;
        public event Action<ICard> OnRemove;
        public int Count { get; private set; }


        public void AddCard(ICard card)
        {
            Count++;
            OnAdd?.Invoke(card);
        }

        public void RemoveCard(ICard card)
        {
            Count--;
            OnRemove?.Invoke(card);
        }

        public void Clear()
        {
            for (int i = Count - 1; i >= 0; i--) RemoveCard(null);
        }

        public bool Contains(ICard card)
        {
            return false;
        }

        public ICard PopCard()
        {
            Count--;
            return null;
        }
    }
}