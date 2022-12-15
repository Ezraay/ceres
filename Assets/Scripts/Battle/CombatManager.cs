using System.Linq;

namespace CardGame
{
    public class CombatManager
    {
        public bool ValidAttack => Attacker != null;
        public readonly MultiCardSlot Defenders = new MultiCardSlot();
        private CardSlot target;
        public CardSlot Attacker;
        private readonly int damage = 1;

        public void AddAttacker(CardSlot slot)
        {
            Attacker = slot;
            slot.Exhaust();
        }

        public void AddTarget(CardSlot slot)
        {
            target = slot;
        }

        public void AddDefender(ICard card)
        {
            Defenders.AddCard(card);
        }

        public void Reset(MultiCardSlot graveyard)
        {
            Attacker = null;
            foreach (var defender in Defenders.Cards) graveyard.AddCard(defender);
            Defenders.Clear();
        }

        public int DamageCount()
        {
            int attack = Attacker.Card.Data.Attack;
            int defense = Defenders.Cards.Sum(card => card.Data.Defense);
            return defense + target.Card.Data.Attack > attack ? 0 : damage;
        }
    }
}