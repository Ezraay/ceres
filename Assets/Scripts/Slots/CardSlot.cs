using System;

namespace CardGame
{
    public class CardSlot : ISlot
    {
        public Card Card { get; private set; }
        public bool Exhausted { get; private set; }
        public event Action<Card> OnChange;
        public event Action OnExhaust;
        public event Action OnAlert;

        public CardSlot(Card card = null)
        {
            Card = card;
        }

        public void Exhaust()
        {
            Exhausted = true;
            OnExhaust?.Invoke();
        }

        public void Alert()
        {
            Exhausted = false;
            OnAlert?.Invoke();
        }

        public void SetCard(Card card, bool exhausted = false)
        {
            Card = card;
            Exhausted = exhausted;
            OnChange?.Invoke(Card);
        }
    }
}