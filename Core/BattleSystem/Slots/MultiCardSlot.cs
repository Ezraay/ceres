using System;
using System.Collections.Generic;

namespace Ceres.Core.BattleSystem
{
    public class MultiCardSlot : Slot, IMultiCardSlot
    {
        public List<Card> Cards { get; }

        public int Count => Cards.Count;
        public event Action<Card> OnAdd;
        public event Action<Card> OnRemove;

        public MultiCardSlot(List<Card> cards = null)
        {
            Cards = cards ?? new List<Card>();
        }

        public void AddCard(Card card)
        {
            Cards.Add(card);
            OnAdd?.Invoke(card);
        }

        public Card GetCard(Guid id)
        {
            foreach (Card card in Cards)
                if (card.ID == id)
                    return card;

            return null;
        }

        public void RemoveCard(Card card)
        {
            Cards.Remove(card);
            OnRemove?.Invoke(card);
        }

        public void Clear()
        {
            for (int i = Cards.Count - 1; i >= 0; i--) RemoveCard(Cards[i]);
        }

        public bool Contains(Card card)
        {
            return Cards.Contains(card);
        }

        public Card PopCard()
        {
            if (Cards.Count == 0) return null;

            Card card = Cards[0];
            Cards.RemoveAt(0);
            OnRemove?.Invoke(card);
            return card;
        }
    }
}