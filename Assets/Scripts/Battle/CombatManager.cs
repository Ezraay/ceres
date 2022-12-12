namespace CardGame
{
    public class CombatManager
    {
        public bool ValidAttack => attacker != null;
        public readonly MultiCardSlot Defenders = new MultiCardSlot();
        private CardSlot attacker;
        private int damage = 1;

        public void AddAttacker(CardSlot slot)
        {
            attacker = slot;
            slot.Exhaust();
        }

        public void AddDefender(Card card)
        {
            Defenders.AddCard(card);
        }

        public void Reset(MultiCardSlot graveyard)
        {
            attacker = null;
            foreach (var defender in Defenders.Cards)
            {
                graveyard.AddCard(defender);
            }
            Defenders.Clear();
        }

        public int DamageCount()
        {
            return Defenders.Cards.Count > 0 ? 0 : damage;
        }
    }
}