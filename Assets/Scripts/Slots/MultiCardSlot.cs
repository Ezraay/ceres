using System;
using System.Collections.Generic;

namespace CardGame
{
    public class MultiCardSlot : ISlot
    {
        public List<ICard> Cards;
        public event Action<ICard> OnAdd;
        public event Action<ICard> OnRemove;

        public MultiCardSlot(List<ICard> cards = null)
        {
            Cards = cards ?? new List<ICard>();
        }

        public ICard AddCard(ICard card)
        {
            Cards.Add(card);
            OnAdd?.Invoke(card);
            return card;
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