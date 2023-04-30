using Ceres.Core.BattleSystem.Battles;

namespace Ceres.Core.BattleSystem
{
    public class AdvancePhaseCommand : ClientCommand
    {
        public override bool CanExecute(Battle battle, IPlayer author)
        {
            if (battle.PhaseManager.Phase == BattlePhase.Defend) return author != battle.PhaseManager.CurrentTurnPlayer;
            return author == battle.PhaseManager.CurrentTurnPlayer;
        }

        public override void Apply(ServerBattle battle, IPlayer author)
        {
            battle.PhaseManager.Advance();
        }

        public override ServerAction[] GetActionsForAlly(IPlayer author)
        {
            return new ServerAction[] {new AdvancePhaseAction()};
        }
    }
}