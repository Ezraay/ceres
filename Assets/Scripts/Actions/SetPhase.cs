namespace CardGame
{
    public class SetPhase : IAction
    {
        private readonly BattlePhase phase;

        public SetPhase(BattlePhase phase)
        {
            this.phase = phase;
        }
        
        public bool CanExecute(Battle battle, Player player)
        {
            return true;
        }

        public void Execute(Battle battle, Player player)
        {
            battle.Phase.Set(phase);
        }
    }
}