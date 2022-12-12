namespace CardGame
{
    public class AttackManager
    {
        public bool ValidAttack => attacker != null;
        private CardSlot attacker;

        public void AddAttacker(CardSlot slot)
        {
            attacker = slot;
            slot.Exhaust();
        }
    }
}