namespace Ceres.Core.BattleSystem
{
    public class AdvancePhaseCommand : IClientCommand
    {
        public bool CanExecute(ClientBattle battle)
        {
            return battle.IsPriorityPlayer();
        }

        public bool CanExecute(ServerBattle battle, IPlayer author)
        {
            return battle.IsPriorityPlayer(author);
        }

        public void Apply(ServerBattle battle, IPlayer author)
        {
            battle.PhaseManager.Advance();
        }

        public IServerAction[] GetActionsForAlly()
        {
            return new IServerAction[] {new AdvancePhaseAction()};
        }

        public IServerAction[] GetActionsForOpponent()
        {
            return GetActionsForAlly();
        }
    }
}