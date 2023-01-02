using System.Collections.Generic;
using Ceres.Core.BattleSystem.Cards;
using Ceres.Core.BattleSystem.Slots;

namespace Ceres.Core.BattleSystem.Players
{
    

    public class Player : IPlayer
    {
        public CardSlot Champion { get; }
        public IMultiCardSlot Pile { get; }
        public IMultiCardSlot Hand { get; } = new MultiCardSlot();
        public IMultiCardSlot Graveyard { get; } = new MultiCardSlot();
        public IMultiCardSlot Damage { get; } = new MultiCardSlot();

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