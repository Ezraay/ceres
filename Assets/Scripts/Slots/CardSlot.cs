using System;

namespace CardGame.Slots
{
    public class CardSlot
    {
        public Card Card { get; private set; }
        public event Action<Card> OnChange;

        public void SetCard(Card card)
        {
            Card = card;
            OnChange?.Invoke(Card);
        }
    }
}