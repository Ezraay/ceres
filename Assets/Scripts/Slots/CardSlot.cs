using System;

namespace CardGame
{
    public class CardSlot : ISlot
    {
        public ICard Card { get; private set; }
        public bool Exhausted { get; private set; }
        public event Action<ICard> OnChange;
        public event Action OnExhaust;
        public event Action OnAlert;

        public CardSlot(ICard card = null)
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

        public void SetCard(ICard card, bool exhausted = false)
        {
            Card = card;
            Exhausted = exhausted;
            OnChange?.Invoke(Card);
        }

        public void ClearCard()
        {
            Card = null;
            OnChange?.Invoke(null);
        }
    }
}