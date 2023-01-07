using System.Linq;
using Ceres.Core.OldBattleSystem.Cards;
using Ceres.Core.OldBattleSystem.Slots;

namespace Ceres.Core.OldBattleSystem.Battles
{
    public class CombatManager
    {
        public bool ValidAttack => Attacker != null;
        public readonly MultiCardSlot Defenders = new MultiCardSlot();
        public CardSlot Target { get; private set; }
        public CardSlot Attacker { get; private set; }
        private readonly int damage = 1;

        public void AddAttacker(CardSlot slot)
        {
            Attacker = slot;
            slot.Exhaust();
        }

        public void AddTarget(CardSlot slot)
        {
            Target = slot;
        }

        public void AddDefender(ICard card)
        {
            if (card != null)
                Defenders.AddCard(card);
        }

        public void Reset(IMultiCardSlot graveyard)
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