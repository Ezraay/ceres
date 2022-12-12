using System;

namespace CardGame.Slots
{
    public class CardSlot : ISlot
    {
        public Card Card { get; private set; }
        public event Action<Card> OnChange;

        public CardSlot(Card card = null)
        {
            Card = card;
        }
        
        public void SetCard(Card card)
        {
            Card = card;
            OnChange?.Invoke(Card);
        }
    }
}