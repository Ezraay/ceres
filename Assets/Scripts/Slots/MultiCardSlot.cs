using System;
using System.Collections.Generic;

namespace CardGame
{
    public class MultiCardSlot : ISlot
    {
        public List<Card> Cards;
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

        public void RemoveCard(Card card)
        {
            Cards.Remove(card);
            OnRemove?.Invoke(card);
        }

        public void Clear()
        {
            for (int i = Cards.Count - 1; i >= 0; i--) RemoveCard(Cards[i]);
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