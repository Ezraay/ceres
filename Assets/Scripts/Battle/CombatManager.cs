using System.Linq;

namespace CardGame
{
    public class CombatManager
    {
        public bool ValidAttack => attacker != null;
        public readonly MultiCardSlot Defenders = new MultiCardSlot();
        private CardSlot target;
        private CardSlot attacker;
        private readonly int damage = 1;

        public void AddAttacker(CardSlot slot)
        {
            attacker = slot;
            slot.Exhaust();
        }

        public void AddTarget(CardSlot slot)
        {
            target = slot;
        }

        public void AddDefender(Card card)
        {
            Defenders.AddCard(card);
        }

        public void Reset(MultiCardSlot graveyard)
        {
            attacker = null;
            foreach (var defender in Defenders.Cards) graveyard.AddCard(defender);
            Defenders.Clear();
        }

        public int DamageCount()
        {
            int attack = attacker.Card.Data.Attack;
            int defense = Defenders.Cards.Sum(card => card.Data.Defense);
            return defense + target.Card.Data.Attack > attack ? 0 : damage;
        }
    }
}