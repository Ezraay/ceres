namespace CardGame
{
    public class AdvancePhase : IAction
    {
        public bool CanExecute(Battle battle, Player player)
        {
            return player == battle.PriorityPlayer;
        }

        public void Execute(Battle battle, Player player)
        {
            battle.BattlePhaseManager.Advance();
        }
    }
}