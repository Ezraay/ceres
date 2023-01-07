﻿using System;
using Ceres.Core.OldBattleSystem.Cards;

namespace Ceres.Core.OldBattleSystem.Slots
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

        public ICard GetCard(Guid id)
        {
            return new NullCard();
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
            return new NullCard();
        }
    }
}