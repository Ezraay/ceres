namespace CardGame
{
    public class DeclareAttack : IAction
    {
        private readonly CardSlot slot;

        public DeclareAttack(CardSlot slot)
        {
            this.slot = slot;
        }
        
        public bool CanExecute(Battle battle, Player player)
        {
            return battle.Phase == BattlePhase.Attack && player == battle.PriorityPlayer;
        }

        public void Execute(Battle battle, Player player)
        {
            battle.CombatManager.AddAttacker(slot);
        }
    }
}