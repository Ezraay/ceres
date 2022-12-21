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

        public Player(List<ICard> pile, ICard champion)
        {
            Pile = new MultiCardSlot(pile);
            Champion = new CardSlot(champion);
        }

        public Player()
        {
            Pile = new MultiCardSlot();
            Champion = new CardSlot();
        }

        public void PreGameSetup()
        {
            for (int i = 0; i < 6; i++) 
                Hand.AddCard(Pile.PopCard());
        }
    }
}