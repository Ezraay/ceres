using System.Linq;

namespace Ceres.Core.BattleSystem
{
    public class CombatManager
    {
        public bool ValidAttack => Attacker != null;
        public UnitSlot Target { get; private set; }
        public UnitSlot Attacker { get; private set; }
        private readonly int damage = 1;
        public readonly MultiCardSlot Defenders = new MultiCardSlot();

        public void AddAttacker(UnitSlot slot)
        {
            Attacker = slot;
            slot.Exhaust();
        }

        public void AddTarget(UnitSlot slot)
        {
            Target = slot;
        }

        public void AddDefender(Card card)
        {
            if (card != null)
                Defenders.AddCard(card);
        }

        public void Reset(MultiCardSlot graveyard)
        {
            Attacker = null;
            Target = null;
            foreach (var defender in Defenders.Cards) graveyard.AddCard(defender);
            Defenders.Clear();
        }

        public int DamageCount()
        {
            if (!ValidAttack) return 0;
            int attack = Attacker.Card.Data.Attack;
            int defense = Defenders.Cards.Sum(card => card.Data.Defense);
            return defense + Target.Card.Data.Attack > attack ? 0 : damage;
        }
    }
}