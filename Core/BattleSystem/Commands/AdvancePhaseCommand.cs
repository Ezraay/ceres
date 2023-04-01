using Ceres.Core.BattleSystem.Battles;

namespace Ceres.Core.BattleSystem
{
    public class AdvancePhaseCommand : IClientCommand
    {
        public bool CanExecute(ClientBattle battle, IPlayer author)
        {
            return battle.HasPriority(author);
        }

        public bool CanExecute(ServerBattle battle, IPlayer author)
        {
            return battle.HasPriority(author);
        }

        public void Apply(ServerBattle battle, IPlayer author)
        {
            battle.PhaseManager.Advance();
        }

        public IServerAction[] GetActionsForAlly(IPlayer author)
        {
            return new IServerAction[] {new AdvancePhaseAction()};
        }

        public IServerAction[] GetActionsForOpponent(IPlayer author)
        {
            return GetActionsForAlly(null);
        }
    }
}