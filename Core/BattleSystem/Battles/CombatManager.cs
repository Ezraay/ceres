using System.Linq;

namespace Ceres.Core.BattleSystem
{
    public class CombatManager
    {
        public bool ValidAttack => Attacker != null && Target != null;
        public UnitSlot Target { get; private set; }
        public UnitSlot Attacker { get; private set; }
        private readonly int damage = 1;
        public readonly MultiCardSlot Defenders = new MultiCardSlot();
        public IPlayer TargetPlayer { get; private set; }

        public void AddAttacker(UnitSlot slot)
        {
            Attacker = slot;
            slot.Exhaust();
        }

        public void AddTarget(UnitSlot slot, IPlayer target)
        {
            Target = slot;
            TargetPlayer = target;
        }

        public void AddDefender(Card card)
        {
            if (card != null)
                Defenders.AddCard(card);
        }

        public void Reset()
        {
            Attacker = null;
            Target = null;
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