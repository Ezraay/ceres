using System.Collections.Generic;
using CardGame.Slots;

namespace CardGame
{
    public class Player
    {
        public CardSlot Champion { get; } = new CardSlot();
        public MultiCardSlot Pile { get; }
        public MultiCardSlot Hand { get; } = new MultiCardSlot();

        public Player(List<Card> Pile)
        {
            this.Pile = new MultiCardSlot(Pile);
        }
    }
}