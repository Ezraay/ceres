using System;
using System.Collections.Generic;
using Ceres.Core.BattleSystem.Cards;

namespace Ceres.Core.BattleSystem.Slots
{
    public class MultiCardSlot : IMultiCardSlot
    {
        public List<ICard> Cards { get; }

        public int Count => Cards.Count;
        public event Action<ICard> OnAdd;
        public event Action<ICard> OnRemove;

        public MultiCardSlot(List<ICard> cards = null)
        {
            Cards = cards ?? new List<ICard>();
        }

        public void AddCard(ICard card)
        {
            Cards.Add(card);
            OnAdd?.Invoke(card);
        }

        public void RemoveCard(ICard card)
        {
            Cards.Remove(card);
            OnRemove?.Invoke(card);
        }

        public void Clear()
        {
            for (int i = Cards.Count - 1; i >= 0; i--) RemoveCard(Cards[i]);
        }

        public bool Contains(ICard card)
        {
            return Cards.Contains(card);
        }

        public ICard PopCard()
        {
            if (Cards.Count == 0) return null;

            ICard card = Cards[0];
            Cards.RemoveAt(0);
            OnRemove?.Invoke(card);
            return card;
        }
    }
}