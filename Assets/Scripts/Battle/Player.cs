using System.Collections.Generic;

namespace CardGame
{
    public class Player
    {
        public CardSlot Champion { get; }
        public MultiCardSlot Pile { get; }
        public MultiCardSlot Hand { get; } = new MultiCardSlot();
        public MultiCardSlot Graveyard { get; } = new MultiCardSlot();
        public MultiCardSlot Damage { get; } = new MultiCardSlot();

        public Player(List<Card> pile, Card champion)
        {
            Pile = new MultiCardSlot(pile);
            Champion = new CardSlot(champion);
        }
    }
}