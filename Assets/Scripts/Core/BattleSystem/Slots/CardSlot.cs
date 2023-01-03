using System;
using Ceres.Core.BattleSystem.Cards;

namespace Ceres.Core.BattleSystem.Slots
{
    public class CardSlot : ISlot
    {
        public ICard Card { get; private set; }
        public bool Exhausted { get; private set; }
        public readonly int x;
        public readonly int y;
        public event Action<ICard> OnChange;
        public event Action OnExhaust;
        public event Action OnAlert;

        public CardSlot(int x, int y, ICard card = null)
        {
            this.x = x;
            this.y = y;
            Card = card;
        }

        public CardSlot(ICard card = null) : this(-1, -1, card) { }

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