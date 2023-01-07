using System.Collections.Generic;
using Ceres.Core.OldBattleSystem.Cards;
using Ceres.Core.OldBattleSystem.Slots;

namespace Ceres.Core.OldBattleSystem.Players
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
            Champion = new CardSlot(0, 0, champion);
        }

        public Player() : this(null, null) { }

        public CardSlot GetCardSlot(int x, int y)
        {
            foreach (CardSlot slot in GetAllCardSlots())
            {
                if (slot.x == x && slot.y == y)
                    return slot;
            }

            return null;
        }

        public List<CardSlot> GetAllCardSlots()
        {
            return new List<CardSlot>()
            {
                Champion
            };
        }
        
        public void PreGameSetup()
        {
            for (int i = 0; i < 6; i++)
                Hand.AddCard(Pile.PopCard());
        }
    }
}