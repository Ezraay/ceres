using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem
{
    public class MultiCardSlot : Slot, IMultiCardSlot
    {
        public List<Card> Cards;

        [JsonIgnore] public int Count => Cards.Count;
        public event Action<Card> OnAdd;
        public event Action<Card> OnRemove;

        public MultiCardSlot()
        {
            Cards = new List<Card>();
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
            RemoveCard(card);
            return card;
        }

        public Card PopRandomCard()
        {
            if (Cards.Count == 0) return null;
            
            Random random = new Random();
            int index = random.Next(0, Cards.Count);
            Card card = Cards[index];
            RemoveCard(card);
            return card;
        }
    }
}